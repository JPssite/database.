using System;
using System.Web;
using System.Web.UI;

namespace myfirst_sql_practice
{
    public static class AuthHelper
    {
        public static bool IsLoggedIn
        {
            get { return HttpContext.Current.Session["Username"] != null; }
        }

        public static bool IsAdmin
        {
            get
            {
                string role = HttpContext.Current.Session["Role"] as string;
                return string.Equals(role, "admin", StringComparison.OrdinalIgnoreCase);
            }
        }

        // Para sa pages na pwede ang kahit sinong naka-login (student o admin)
        public static void RequireLogin(Page page)
        {
            if (!IsLoggedIn)
                page.Response.Redirect("~/login.aspx");
        }

        // Para sa pages na admin lang
        public static void RequireAdmin(Page page)
        {
            if (!IsLoggedIn)
                page.Response.Redirect("~/login.aspx");
            else if (!IsAdmin)
                page.Response.Redirect("~/paymentstatus.aspx");
        }

        public static void Logout(Page page)
        {
            page.Session.Clear();
            page.Session.Abandon();
            page.Response.Redirect("~/login.aspx");
        }
    }
}