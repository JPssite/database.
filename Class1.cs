using System;
using System.Web;
using System.Web.UI;

namespace myfirst_sql_practice
{
    // Lahat ng SweetAlert message ay dito dadaan.
    // Gamit sa kahit anong page:  SweetAlert.Success(this, "Saved");
    public static class SweetAlert
    {
        private const string SCRIPT_PATH = "~/Scripts/sweetalert2_11.js";
        private const string PRIMARY_COLOR = "#4f46e5";

        public static void Success(Page page, string message)
        {
            Show(page, "success", "Success", message);
        }

        public static void Warning(Page page, string message)
        {
            Show(page, "warning", "Warning", message);
        }

        public static void Error(Page page, string message)
        {
            Show(page, "error", "Error", message);
        }

        public static void Info(Page page, string message)
        {
            Show(page, "info", "Info", message);
        }

        public static void Show(Page page, string icon, string title, string message)
        {
            // I-load ang sweetalert2 file (isang beses lang kahit ilang beses tawagin)
            page.ClientScript.RegisterClientScriptInclude(
                page.GetType(), "sweetalert2", page.ResolveUrl(SCRIPT_PATH));

            // Para hindi masira ang script kahit may ' o " sa message
            string safeTitle = HttpUtility.JavaScriptStringEncode(title);
            string safeMessage = HttpUtility.JavaScriptStringEncode(message);

            // May backup na normal alert kung sakaling hindi ma-load ang sweetalert file
            string script =
                "if (window.Swal) {" +
                "  Swal.fire({ icon: '" + icon + "', title: '" + safeTitle + "', text: '" + safeMessage + "', confirmButtonColor: '" + PRIMARY_COLOR + "' });" +
                "} else { alert('" + safeMessage + "'); }";

            page.ClientScript.RegisterStartupScript(
                page.GetType(), Guid.NewGuid().ToString(), script, true);
        }
    }
}