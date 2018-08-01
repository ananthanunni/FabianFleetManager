namespace FleetManagerWeb
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/Style/login").Include(
                "~/Content/Style/style.css",
                "~/Scripts/Library/bootstrap/bootstrap-responsive.min.css",
                "~/Scripts/Library/bootstrap/bootstrap.css",
                "~/Scripts/Library/jAlert/jquery.alerts.css",
                "~/Scripts/Library/chosen/chosen.css",
                "~/Scripts/Library/tipsy/tipsy.css",
                "~/Scripts/Library/uniform/uniform.default.css"));

            bundles.Add(new ScriptBundle("~/Scripts/login").Include(
                "~/Scripts/JQuery/jquery-1.11.0.js",
                "~/Scripts/JQuery/jquery-migrate-1.2.1.js",
                "~/Scripts/Library/tipsy/jquery.tipsy.js",
                "~/Scripts/Library/uniform/jquery.uniform.js",
                "~/Scripts/Library/jAlert/jquery.alerts.js",
                "~/Scripts/Library/chosen/chosen.jquery.js",
                "~/Scripts/Utility/Login.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Js").Include(
                "~/Scripts/JQuery/jquery-1.11.0.js",
                "~/Scripts/JQuery/jquery-migrate-1.2.1.js",
                "~/Scripts/JQuery/jquery.debouncedresize.js",
                "~/Scripts/JQuery/jquery-ui-1.10.4.custom.js",
                "~/Scripts/JQuery/jquery.jqGrid.src.js",
                "~/Scripts/JQuery/grid.locale-en.js",
                "~/Scripts/Library/bootstrap/bootstrap.js",
                "~/Scripts/Library/tipsy/jquery.tipsy.js",
                "~/Scripts/Library/uniform/jquery.uniform.js",
                "~/Scripts/Library/inputmask/jquery.maskedinput-1.3.js",
                "~/Scripts/Library/timepicker/jquery-ui-timepicker.js",
                "~/Scripts/Library/chosen/chosen.jquery.js",
                "~/Scripts/Library/jAlert/jquery.alerts.js",
                "~/Scripts/Utility/Common.js"));

            BundleTable.EnableOptimizations = false;
        }
    }
}