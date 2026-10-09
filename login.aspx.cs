using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace myfirst_sql_practice
{
    public partial class login : System.Web.UI.Page
    {
        public MySqlConnection con = new MySqlConnection(
            ConfigurationManager.ConnectionStrings["mycon1"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string user = txtuser.Text.Trim();
            string pass = txtpass.Text;

            if (user == "" || pass == "")
            {
                SweetAlert.Warning(this, "Please enter username and password");
                return;
            }

            string fullname = null;
            string username = null;
            string storedHash = null;
            string role = "student";

            try
            {
                string query = "SELECT fullname, username, `password`, role FROM tblaccounts WHERE username = ?user";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("?user", user);
                    con.Open();

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            fullname = dr["fullname"].ToString();
                            username = dr["username"].ToString();
                            storedHash = dr["password"].ToString();
                            role = dr["role"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SweetAlert.Error(this, "Error: " + ex.Message);
                return;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            // walang account na ganoong username, o mali ang password
            if (storedHash == null || !PasswordHelper.Verify(pass, storedHash))
            {
                SweetAlert.Error(this, "Invalid username or password");
                txtpass.Focus();
                return;
            }

            Session["FullName"] = fullname;
            Session["Username"] = username;
            Session["Role"] = role;
            Session["LoginTime"] = DateTime.Now;
            Session["JustLoggedIn"] = true;

            if (role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                Response.Redirect("~/PaymentForm.aspx");
            else
                Response.Redirect("~/paymentstatus.aspx");
        }

        protected void register_Account_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/register_account.aspx");
        }
    }
}