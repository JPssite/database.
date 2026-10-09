using MySql.Data.MySqlClient;
using myfirst_sql_practice;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace MyFirstdbApp
{
    public partial class Add_Student : System.Web.UI.Page
    {
        public MySqlConnection con = new MySqlConnection(
            ConfigurationManager.ConnectionStrings["mycon1"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthHelper.RequireAdmin(this);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtname.Text.Trim();

            if (name == "")
            {
                SweetAlert.Warning(this, "Please enter student name");
                txtname.Focus();
                return;
            }

            try
            {
                con.Open();

                // 1. Check kung existing na ang pangalan
                string querycheck = "SELECT COUNT(*) FROM tblstudents WHERE fullname = ?name";
                using (MySqlCommand cmdCheck = new MySqlCommand(querycheck, con))
                {
                    cmdCheck.Parameters.AddWithValue("?name", name);
                    int exists = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (exists > 0)
                    {
                        SweetAlert.Warning(this, "This student name already exists in the database");
                        txtname.Focus();
                        return;   // hindi na itutuloy ang INSERT
                    }
                }

                // 2. Walang kapareho, i-save na
                string queryins = @"INSERT INTO tblstudents (fullname, contactno)
                                    VALUES (?name, ?contact)";

                using (MySqlCommand cmd = new MySqlCommand(queryins, con))
                {
                    cmd.Parameters.AddWithValue("?name", name);
                    cmd.Parameters.AddWithValue("?contact", txtcontact.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                txtname.Text = "";
                txtcontact.Text = "";
                txtname.Focus();

                SweetAlert.Success(this, "Student saved");
            }
            catch (Exception ex)
            {
                SweetAlert.Error(this, "Error on saving: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        // PAYMENT button: punta sa payment form
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PaymentForm.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthHelper.Logout(this);
        }
    }
}