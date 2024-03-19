using System.Web;
using System.Web.Optimization;

namespace Tms.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Assets/js/bootstrap-datepicker.js",
                      "~/Assets/js/notify.min.js",
                      "~/Assets/plugins/sweetalert/dist/sweetalert2.min.js",
                      "~/Scripts/respond.js",
                      "~/Assets/scripts/Tms.lib.js",
                      "~/Assets/scripts/Tms.control.js",
                      "~/Assets/scripts/Tms.client.js",
                      "~/Assets/scripts/Tms.role.js",
                      "~/Assets/scripts/Tms.account.js",
                      "~/Assets/js/main.js",
                      "~/Assets/js/nav.js",
                       "~/Assets/js/custom.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jscommon").Include(
                     "~/Assets/scripts/Tms.common.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"
                      ));

            bundles.Add(new StyleBundle("~/admin-bundles/csshome").Include(
                        "~/Assets/css/jquery-ui.css",
                        "~/Content/css/bootstrap.min.css",
                        "~/Assets/css/datepicker.css",
                        "~/Assets/css/londinium-theme.css",
                        "~/Assets/css/style.css",
                        "~/Assets/css/icons.css",
                        "~/Assets/css/custom.css",
                        "~/Assets/css/select2.css",
                          "~/Assets/css/font.css",
                        "~/Assets/plugins/sweetalert/dist/sweetalert2.min.css",
                        "~/Content/fonts/css/all.css",
                        "~/Assets/css/custom.css",
                        "~/Assets/css/style1.css",
                        "~/Assets/css/stylecopy.css",
                        "~/Assets/css/styleh.css"
                      ));
        }
    }
}
