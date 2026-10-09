using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace myfirst_sql_practice
{
    public partial class timedatepayment : System.Web.UI.Page
    {
        public MySqlConnection con = new MySqlConnection(
            ConfigurationManager.ConnectionStrings["mycon1"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthHelper.RequireLogin(this);

            // student: Payment Status at Payment History lang ang makikita
            btnRegister.Visible = AuthHelper.IsAdmin;
            btnPayment.Visible = AuthHelper.IsAdmin;

            if (!IsPostBack)
            {
                LoadStudents();
            }
        }

        protected void drpStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHistory();
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            AuthHelper.RequireAdmin(this);
            Response.Redirect("~/register_name.aspx");
        }

        protected void btnPayment_Click(object sender, EventArgs e)
        {
            AuthHelper.RequireAdmin(this);
            Response.Redirect("~/PaymentForm.aspx");
        }

        protected void btnStatus_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/paymentstatus.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthHelper.Logout(this);
        }

        private void LoadStudents()
        {
            string query = "SELECT studentid, fullname FROM tblstudents ORDER BY fullname";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        drpStudent.Items.Clear();
                        drpStudent.Items.Add(new ListItem("-- Select Student --", "0"));

                        while (dr.Read())
                        {
                            drpStudent.Items.Add(new ListItem(dr["fullname"].ToString(), dr["studentid"].ToString()));
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

        private void LoadHistory()
        {
            // Walang napiling student, i-clear ang grid
            if (drpStudent.SelectedValue == "0")
            {
                GridViewHistory.DataSource = null;
                GridViewHistory.DataBind();
                pnlSummary.Visible = false;
                return;
            }

            // ROW_NUMBER = pang-ilang beses nang nagbayad ang student (MySQL 8.0)
            string query = @"SELECT s.fullname,
                                    f.fees,
                                    ROW_NUMBER() OVER (ORDER BY t.Datepayed) AS paymentno,
                                    t.amountpay,
                                    t.Datepayed
                             FROM tblpayment t
                             INNER JOIN tblstudents s ON s.studentID = t.studentID
                             LEFT JOIN tblfees f ON f.feesID = t.feesID
                             WHERE t.studentID = ?studID
                             ORDER BY t.Datepayed";

            DataTable dt = new DataTable();

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("?studID", drpStudent.SelectedValue);

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        con.Open();
                        da.Fill(dt);
                    }
                }

                GridViewHistory.DataSource = dt;
                GridViewHistory.DataBind();

                if (dt.Rows.Count > 0)
                {
                    DateTime first = Convert.ToDateTime(dt.Rows[0]["Datepayed"]);
                    DateTime last = Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1]["Datepayed"]);

                    lblSummary.Text = "<b>" + dt.Rows.Count + "</b> payment(s) &nbsp;|&nbsp; First: <b>" +
                                      first.ToString("MMM dd, yyyy hh:mm tt") + "</b> &nbsp;|&nbsp; Latest: <b>" +
                                      last.ToString("MMM dd, yyyy hh:mm tt") + "</b>";
                    pnlSummary.Visible = true;

                    ShowFooterTotals(dt, last);
                }
                else
                {
                    pnlSummary.Visible = false;
                    SweetAlert.Warning(this, "No payments found for this student");
                }
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
        }

        // Footer: total payments, total amount na nabayaran, at petsa ng huling bayad
        private void ShowFooterTotals(DataTable dt, DateTime last)
        {
            if (GridViewHistory.FooterRow == null) return;

            decimal total = 0;
            foreach (DataRow r in dt.Rows)
                total += Convert.ToDecimal(r["amountpay"]);

            GridViewRow footer = GridViewHistory.FooterRow;
            footer.Cells[0].Text = "TOTAL";
            footer.Cells[1].Text = "";
            footer.Cells[2].Text = dt.Rows.Count + " payment(s)";
            footer.Cells[3].Text = total.ToString("N2");
            footer.Cells[4].Text = "Last: " + last.ToString("MMM dd, yyyy hh:mm tt");
        }
    }
}