using System.Web;
using System.Web.Optimization;

namespace edueTree
{
    public class BundleConfig
    {        
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/app.min.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/modernizr.custom.80028.js",
                      "~/Scripts/bootstrap-datetimepicker.min.js",
                      "~/Scripts/bootstrapValidator.min.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/fullcalendar.js",
                      "~/Scripts/fullcalendar.min.js"
                      ));
             bundles.Add(new ScriptBundle("~/bundles/datatablejs").Include("~/plugins/datatables/jquery.dataTables.min.js",
                     "~/plugins/datatables/dataTables.bootstrap.min.js",
                     "~/plugins/select2/select2.full.js"
                   ));

            bundles.Add(new StyleBundle("~/Content/datatablecss").Include( "~/plugins/datatables/dataTables.bootstrap.css",
                "~/js/datatablejs/buttons.dataTables.min.css",
                "~/bootstrap/css/bootstrap.min.css",
                "~/Content/AdminLTE.min.css",
                "~/Content/_all-skins.min.css",
                "~/Content/bootstrap-datepicker.min.css"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/datatableprintjs").Include("~/js/datatablejs/buttons.flash.min.js",
            "~/js/datatablejs/jszip.min.js",
            "~/js/datatablejs/pdfmake.min.js",
            "~/js/datatablejs/vfs_fonts.js",
            "~/js/datatablejs/buttons.html5.min.js",
            "~/js/datatablejs/buttons.print.min.js",
            "~/js/datatablejs/raphael-min.js",
            "~/js/notify.min.js",
            "~/js/dataTables.buttons.min.js"
          ));
       //     bundles.Add(new StyleBundle("~/bundles/datatablecss").Include("~/js/datatablejs/buttons.dataTables.min.css",
       //         "~/js/font-awesome.min.css",
       //         "~/js/ionicons.min.css",
       //         "~/bootstrap/css/bootstrap.min.css",
       //         "~/Content/AdminLTE.min.css",
       //         "~/Content/_all-skins.min.css",
       //         "~/Content/bootstrap-datepicker.min.css"
       //));
        }
    }
}
