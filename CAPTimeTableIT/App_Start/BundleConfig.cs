using System.Web;
using System.Web.Optimization;

namespace CAPTimeTableIT
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/timetable").Include(
                      "~/Content/capstone/assign-class.css",
                      "~/Content/capstone/time-table.css"));

            bundles.Add(new ScriptBundle("~/bundles/data-table").Include(
                      "~/res/src/plugins/datatables/js/dataTables.buttons.min.js",
                      "~/res/src/plugins/datatables/js/buttons.bootstrap4.min.js",
                      "~/res/src/plugins/datatables/js/buttons.print.min.js",
                      "~/res/src/plugins/datatables/js/buttons.html5.min.js",
                      "~/res/src/plugins/datatables/js/buttons.flash.min.js",
                      "~/res/src/plugins/datatables/js/pdfmake.min.js",
                      "~/res/src/plugins/datatables/js/vfs_fonts.js",
                      "~/res/vendors/scripts/datatable-setting.js"));
        }
    }
}
