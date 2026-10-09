using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace myfirst_sql_practice
{
    public partial class register_account : System.Web.UI.Page
    {
        public MySqlConnection con = new MySqlConnection(
            ConfigurationManager.ConnectionStrings["mycon1"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string name = Txtname.Text.Trim();
            string user = Txtuser.Text.Trim();
            string pass = Txtpass.Text;

            if (name == "" || user == "" || pass == "")
            {
                SweetAlert.Warning(this, "Please fill in all fields");
                return;
            }

            try
            {
                con.Open();

                // 1. Check kung existing na ang username
                string querycheck = "SELECT COUNT(*) FROM tblaccounts WHERE username = ?user";
                using (MySqlCommand cmdCheck = new MySqlCommand(querycheck, con))
                {
                    cmdCheck.Parameters.AddWithValue("?user", user);
                    int exists = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (exists > 0)
                    {
                        SweetAlert.Warning(this, "Username already exists");
                        Txtuser.Focus();
                        return;
                    }
                }

                // 2. I-save na, naka-hash ang password
                string queryins = @"INSERT INTO tblaccounts (fullname, username, `password`, `datetime`)
                                    VALUES (?name, ?user, ?hash, ?date)";

                using (MySqlCommand cmd = new MySqlCommand(queryins, con))
                {
                    cmd.Parameters.AddWithValue("?name", name);
                    cmd.Parameters.AddWithValue("?user", user);
                    cmd.Parameters.AddWithValue("?hash", PasswordHelper.Hash(pass));
                    cmd.Parameters.AddWithValue("?date", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }

                Txtname.Text = "";
                Txtuser.Text = "";
                Txtpass.Text = "";

                SweetAlert.Success(this, "Account successfully created");
            }
            catch (Exception ex)
            {
                SweetAlert.Error(this, "Error create account: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
}