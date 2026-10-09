using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace myfirst_sql_practice
{
    public partial class PaymentForm : System.Web.UI.Page
    {
        // ====== BAGUHIN ITO KUNG IBA ANG PANGALAN SA DATABASE MO ======
        // Patakbuhin sa Workbench: DESCRIBE tblpayment;  DESCRIBE tblfees;
        const string PAY_TABLE = "tblpayment";
        const string PAY_STUDENT = "StudentID";   // student column sa tblpayment
        const string PAY_FEES = "FeesID";         // gawing "" kung WALANG fees column sa tblpayment
        const string PAY_AMOUNT = "amountpay";    // pwedeng "amount"
        const string PAY_DATE = "Datepayed";      // dapat DATETIME ang type

        const string FEES_TABLE = "tblfees";
        const string FEES_ID = "feesID";
        const string FEES_NAME = "fees";          // pangalan ng fee sa tblfees
        const string FEES_AMOUNT = "amount";      // halaga ng fee sa tblfees

        const string DATE_FORMAT = "yyyy-MM-ddTHH:mm";   // format ng datetime-local textbox
        // ===============================================================

        public MySqlConnection con = new MySqlConnection(
            ConfigurationManager.ConnectionStrings["mycon1"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthHelper.RequireAdmin(this);

            if (!IsPostBack)
            {
                loadstudentToDropdown();
                loadFeesToDropdown();
                txtDatePaid.Text = DateTime.Now.ToString(DATE_FORMAT);
                LoadPayments();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            bool useFees = !string.IsNullOrEmpty(PAY_FEES);

            if (drpstudentlist.SelectedValue == "0")
            {
                SweetAlert.Warning(this, "Please select student");
                return;
            }
            if (useFees && drppaymentlist.SelectedValue == "0")
            {
                SweetAlert.Warning(this, "Please select fees");
                return;
            }

            decimal amount;
            if (!decimal.TryParse(TextBox1.Text.Trim(), out amount) || amount <= 0)
            {
                SweetAlert.Warning(this, "Please enter a valid amount");
                TextBox1.Focus();
                return;
            }

            DateTime datePaid;
            if (!DateTime.TryParseExact(txtDatePaid.Text.Trim(), DATE_FORMAT,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out datePaid))
            {
                SweetAlert.Warning(this, "Please enter a valid date and time");
                txtDatePaid.Focus();
                return;
            }

            try
            {
                string queryins;
                if (useFees)
                {
                    queryins = "INSERT INTO " + PAY_TABLE +
                               " (" + PAY_STUDENT + ", " + PAY_FEES + ", " + PAY_AMOUNT + ", " + PAY_DATE + ")" +
                               " VALUES (?studID, ?feesID, ?amt, ?datepay)";
                }
                else
                {
                    queryins = "INSERT INTO " + PAY_TABLE +
                               " (" + PAY_STUDENT + ", " + PAY_AMOUNT + ", " + PAY_DATE + ")" +
                               " VALUES (?studID, ?amt, ?datepay)";
                }

                using (MySqlCommand cmd = new MySqlCommand(queryins, con))
                {
                    cmd.Parameters.AddWithValue("?studID", drpstudentlist.SelectedValue);
                    if (useFees)
                        cmd.Parameters.AddWithValue("?feesID", drppaymentlist.SelectedValue);
                    cmd.Parameters.AddWithValue("?amt", amount);
                    cmd.Parameters.AddWithValue("?datepay", datePaid);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                SweetAlert.Success(this, "Payment saved");
                TextBox1.Text = "";
                drpstudentlist.SelectedIndex = 0;
                drppaymentlist.SelectedIndex = 0;
                txtDatePaid.Text = DateTime.Now.ToString(DATE_FORMAT);
            }
            catch (Exception ex)
            {
                SweetAlert.Error(this, "Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            // i-refresh ang grid at footer pagkatapos mag-save
            LoadPayments();
        }

        // ADD NEW FEE: nagdadagdag ng bagong fee name at amount sa tblfees
        protected void btnAddFee_Click(object sender, EventArgs e)
        {
            string feeName = txtFeeName.Text.Trim();

            if (feeName == "")
            {
                SweetAlert.Warning(this, "Please enter fee name");
                txtFeeName.Focus();
                return;
            }

            decimal feeAmount;
            if (!decimal.TryParse(txtFeeAmount.Text.Trim(), out feeAmount) || feeAmount <= 0)
            {
                SweetAlert.Warning(this, "Please enter a valid fee amount");
                txtFeeAmount.Focus();
                return;
            }

            bool saved = false;

            try
            {
                con.Open();

                // 1. Check kung existing na ang fee name
                string querycheck = "SELECT COUNT(*) FROM " + FEES_TABLE + " WHERE " + FEES_NAME + " = ?fee";
                using (MySqlCommand cmdCheck = new MySqlCommand(querycheck, con))
                {
                    cmdCheck.Parameters.AddWithValue("?fee", feeName);
                    int exists = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (exists > 0)
                    {
                        SweetAlert.Warning(this, "This fee name already exists");
                        txtFeeName.Focus();
                        return;
                    }
                }

                // 2. Walang kapareho, i-save na
                string queryins = "INSERT INTO " + FEES_TABLE +
                                  " (" + FEES_NAME + ", " + FEES_AMOUNT + ") VALUES (?fee, ?amt)";
                using (MySqlCommand cmd = new MySqlCommand(queryins, con))
                {
                    cmd.Parameters.AddWithValue("?fee", feeName);
                    cmd.Parameters.AddWithValue("?amt", feeAmount);
                    cmd.ExecuteNonQuery();
                }

                saved = true;
            }
            catch (Exception ex)
            {
                SweetAlert.Error(this, "Error on adding fee: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            if (saved)
            {
                txtFeeName.Text = "";
                txtFeeAmount.Text = "";

                // i-reload ang dropdown para lumabas agad ang bagong fee
                loadFeesToDropdown();

                SweetAlert.Success(this, "Fee added");
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthHelper.Logout(this);
        }

        // Grid ng lahat ng payments, may footer na total count at total amount
        private void LoadPayments()
        {
            string query = "SELECT s.fullname, f." + FEES_NAME + " AS fees, " +
                           "t." + PAY_AMOUNT + " AS amountpay, " +
                           "t." + PAY_DATE + " AS Datepayed " +
                           "FROM " + PAY_TABLE + " t " +
                           "INNER JOIN tblstudents s ON s.studentID = t." + PAY_STUDENT + " " +
                           "LEFT JOIN " + FEES_TABLE + " f ON f." + FEES_ID + " = t." + PAY_FEES + " " +
                           "ORDER BY t." + PAY_DATE + " DESC";

            DataTable dt = new DataTable();

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    con.Open();
                    da.Fill(dt);
                }

                GridViewPayments.DataSource = dt;
                GridViewPayments.DataBind();

                if (dt.Rows.Count > 0 && GridViewPayments.FooterRow != null)
                {
                    decimal total = 0;
                    foreach (DataRow r in dt.Rows)
                        total += Convert.ToDecimal(r["amountpay"]);

                    GridViewRow footer = GridViewPayments.FooterRow;
                    footer.Cells[0].Text = "TOTAL";
                    footer.Cells[1].Text = dt.Rows.Count + " payment(s)";
                    footer.Cells[2].Text = total.ToString("N2");
                    footer.Cells[3].Text = "";
                }
            }
            catch (Exception ex)
            {
                SweetAlert.Error(this, "Error loading payments: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void loadstudentToDropdown()
        {
            string query = "SELECT studentid, fullname FROM tblstudents ORDER BY fullname";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        drpstudentlist.Items.Clear();
                        drpstudentlist.Items.Add(new ListItem("-- select Student --", "0"));

                        while (dr.Read())
                        {
                            drpstudentlist.Items.Add(
                                new ListItem(dr["fullname"].ToString(), dr["studentid"].ToString()));
                        }
                    }
                }
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void loadFeesToDropdown()
        {
            // SELECT * para walang "Unknown column" error sa pangalan ng fees
            string query = "SELECT * FROM " + FEES_TABLE;

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        drppaymentlist.Items.Clear();
                        drppaymentlist.Items.Add(new ListItem("-- select Fees --", "0"));

                        // hanapin ang index ng ID column at ng unang ibang column (ang pangalan ng fee)
                        int idIdx = 0;
                        int nameIdx = -1;
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            if (dr.GetName(i).Equals(FEES_ID, StringComparison.OrdinalIgnoreCase))
                                idIdx = i;
                        }
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            if (i != idIdx) { nameIdx = i; break; }
                        }
                        if (nameIdx == -1) nameIdx = idIdx;

                        while (dr.Read())
                        {
                            drppaymentlist.Items.Add(
                                new ListItem(dr[nameIdx].ToString(), dr[idIdx].ToString()));
                        }
                    }
                }
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
}