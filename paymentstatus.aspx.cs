using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace myfirst_sql_practice
{
    public partial class paymentstatus : System.Web.UI.Page
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
                LoadFees();
            }
        }

        protected void drpFees_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudent();
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

        protected void btntimedate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/timedatepayment.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthHelper.Logout(this);
        }

        private void LoadStudent()
        {
            // Walang napiling fees, i-clear ang grid
            if (drpFees.SelectedValue == "0")
            {
                GridViewStudentList.DataSource = null;
                GridViewStudentList.DataBind();
                SweetAlert.Warning(this, "Please select fees");
                return;
            }

            string query = @"SELECT s.fullname, s.contactno,
                                    SUM(t.amountpay) AS totalpaid,
                                    f.fees,
                                    (f.amount - SUM(t.amountpay)) AS balance,
                                    CASE
                                        WHEN (f.amount - SUM(t.amountpay)) <= 0 THEN 'PAID'
                                        ELSE 'NOT YET PAID'
                                    END AS remarks
                             FROM tblpayment t
                             INNER JOIN tblfees f ON f.feesID = t.feesID
                             INNER JOIN tblstudents s ON s.studentID = t.studentID
                             WHERE t.feesID = ?feesID
                             GROUP BY t.studentID, s.fullname, s.contactno, f.fees, f.amount
                             ORDER BY s.fullname";

            DataTable dt = new DataTable();

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("?feesID", drpFees.SelectedValue);

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        con.Open();
                        da.Fill(dt);
                    }
                }

                GridViewStudentList.DataSource = dt;
                GridViewStudentList.DataBind();

                if (dt.Rows.Count == 0)
                {
                    SweetAlert.Warning(this, "No payments found for this fee");
                }
                else
                {
                    ShowFooterTotals(dt);
                    SweetAlert.Success(this, dt.Rows.Count + " student(s) found");
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

        // Footer: total students, total class fund na nakolekta, total balance, ilan PAID / NOT YET PAID
        private void ShowFooterTotals(DataTable dt)
        {
            if (GridViewStudentList.FooterRow == null) return;

            decimal totalPaid = 0;
            decimal totalBalance = 0;
            int paidCount = 0;

            foreach (DataRow r in dt.Rows)
            {
                totalPaid += Convert.ToDecimal(r["totalpaid"]);

                decimal bal = Convert.ToDecimal(r["balance"]);
                if (bal > 0) totalBalance += bal;   // overpaid (negatibo) hindi ibabawas

                if (r["remarks"].ToString() == "PAID") paidCount++;
            }

            int notPaidCount = dt.Rows.Count - paidCount;

            GridViewRow footer = GridViewStudentList.FooterRow;
            
            footer.Cells[1].Text = dt.Rows.Count + " student(s)";
            footer.Cells[2].Text = totalPaid.ToString("N2");
            footer.Cells[3].Text = totalBalance.ToString("N2");
            footer.Cells[4].Text = paidCount + " PAID / " + notPaidCount + " NOT YET PAID";
        }

        private void LoadFees()
        {
            string query = "SELECT feesID, fees FROM tblfees ORDER BY fees";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    con.Open();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        drpFees.Items.Clear();
                        drpFees.Items.Add(new ListItem("--Select Fees--", "0"));

                        while (dr.Read())
                        {
                            drpFees.Items.Add(
                                new ListItem(dr["fees"].ToString(), dr["feesID"].ToString()));
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