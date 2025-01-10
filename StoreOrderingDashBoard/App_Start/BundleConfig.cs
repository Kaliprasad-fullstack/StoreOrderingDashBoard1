using System.Web;
using System.Web.Optimization;

namespace StoreOrderingDashBoard
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
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/Bootstrap").Include(
                "~/Content/vendors/bootstrap/dist/css/bootstrap.min.css",
                "~/Content/vendors/font-awesome/css/font-awesome.min.css",
                "~/Content/vendors/nprogress/nprogress.css",
                "~/Content/vendors/iCheck/skins/flat/green.css"));

            bundles.Add(new StyleBundle("~/Content/DataTable").Include(
                     "~/Content/vendors/datatables.net-bs/css/dataTables.bootstrap.min.css",
                     "~/Content/vendors/datatables.net-buttons-bs/css/buttons.bootstrap.min.css",
                     "~/Content/vendors/datatables.net-fixedheader-bs/css/fixedHeader.bootstrap.min.css",
                     "~/Content/vendors/datatables.net-responsive-bs/css/responsive.bootstrap.min.css",
                     "~/Content/vendors/datatables.net-scroller-bs/css/scroller.bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/ProgressBar").Include(
                    "~/Content/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css"));

            bundles.Add(new StyleBundle("~/Content/JQVMap").Include(
                    "~/Content/vendors/jqvmap/dist/jqvmap.min.css"));

            bundles.Add(new StyleBundle("~/Content/DateRangePicker").Include(
                    "~/Content/vendors/bootstrap-daterangepicker/daterangepicker.css",
                    "~/Content/vendors/bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.css",
                    "~/Content/vendors/switchery/dist/switchery.min.css"));

            bundles.Add(new StyleBundle("~/Content/MultiSelect").Include(
                    "~/Content/build/css/bootstrap-multiselect.css"));


            bundles.Add(new ScriptBundle("~/Scripts/BoostrapJs").Include(
                "~/Content/vendors/jquery/dist/jquery.min.js",
                "~/Content/vendors/bootstrap/dist/js/bootstrap.min.js",
                "~/Content/vendors/fastclick/lib/fastclick.js",
                "~/Content/vendors/nprogress/nprogress.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ChartJs").Include(
                    "~/Content/vendors/Chart.js/dist/Chart.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/GaugeJs").Include(
                    "~/Content/vendors/gauge.js/dist/gauge.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ProgressBarJs").Include(
                    "~/Content/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ICheckJs").Include(
                    "~/Content/vendors/iCheck/icheck.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/SkyconsJs").Include(
                    "~/Content/vendors/skycons/skycons.js"));

            bundles.Add(new ScriptBundle("~/Scripts/FlotJs").Include(
                    "~/Content/vendors/Flot/jquery.flot.js",
                    "~/Content/vendors/Flot/jquery.flot.pie.js",
                    "~/Content/vendors/Flot/jquery.flot.time.js",
                    "~/Content/vendors/Flot/jquery.flot.stack.js",
                    "~/Content/vendors/Flot/jquery.flot.resize.js"));

            bundles.Add(new ScriptBundle("~/Scripts/FlotPlugins").Include(
                    "~/Content/vendors/flot.orderbars/js/jquery.flot.orderBars.js",
                    "~/Content/vendors/flot-spline/js/jquery.flot.spline.min.js",
                    "~/Content/vendors/flot.curvedlines/curvedLines.js"));

            bundles.Add(new ScriptBundle("~/Scripts/DateJs").Include(
                    "~/Content/vendors/DateJS/build/date.js"));

            bundles.Add(new ScriptBundle("~/Scripts/dataTablesJs").Include(
                    "~/Content/vendors/datatables.net/js/jquery.dataTables.min.js",
                    "~/Content/vendors/datatables.net-bs/js/dataTables.bootstrap.min.js",
                    "~/Content/vendors/datatables.net-buttons-bs/js/buttons.bootstrap.min.js",
                    "~/Content/vendors/datatables.net-buttons/js/buttons.flash.min.js",
                    "~/Content/vendors/datatables.net-buttons/js/buttons.print.min.js",
                    "~/Content/vendors/datatables.net-fixedheader/js/dataTables.fixedHeader.min.js",
                    "~/Content/vendors/datatables.net-keytable/js/dataTables.keyTable.min.js",
                    "~/Content/vendors/datatables.net-responsive/js/dataTables.responsive.min.js",
                    "~/Content/vendors/datatables.net-responsive-bs/js/responsive.bootstrap.js",
                    "~/Content/vendors/datatables.net-scroller/js/dataTables.scroller.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/PdfMakeJs").Include(
                    "~/Content/vendors/pdfmake/build/pdfmake.min.js",
                    "~/Content/vendors/pdfmake/build/vfs_fonts.js"));

            bundles.Add(new ScriptBundle("~/Scripts/dataTablesButtonsJs").Include(
                    "~/Content/build/js/dataTables.buttons.min.js",
                    "~/Content/build/js/jszip.min.js",
                    "~/Content/build/js/buttons.html5.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/JQVMapJs").Include(
                   "~/Content/vendors/jqvmap/dist/jquery.vmap.js",    
                   "~/Content/vendors/jqvmap/dist/maps/jquery.vmap.world.js",    
                   "~/Content/vendors/jqvmap/examples/js/jquery.vmap.sampledata.js",    
                   "~/Content/vendors/switchery/dist/switchery.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/DateRangePickerJs").Include(
                    "~/Content/vendors/moment/min/moment.min.js",
                    "~/Content/vendors/bootstrap-daterangepicker/daterangepicker.js",
                    "~/Content/vendors/bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/EChartsJs").Include(
                    "~/Content/vendors/echarts/dist/echarts.min.js",    
                    "~/Content/vendors/echarts/map/js/world.js"));

            bundles.Add(new ScriptBundle("~/Scripts/MultiSelectJs").Include(
                    "~/Content/build/js/bootstrap-multiselect.js"));
        }

    }
}
