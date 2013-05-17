// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// The custom scripts are bundled here along with jQuery. Bundling brings performance
// benefits of a smaller payload and fewer HTTP requests. To enable bundling set
// compilation debug="false" in Web.config. Setting BundleTable.EnableOptimizations 
// = true in this file overrides any Web.config setting.
// ----------------------------------------------------------------------------------
using System.Web;
using System.Web.Optimization;

namespace AzureDoctor
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // This script is on the instructions page.
            bundles.Add(new ScriptBundle("~/bundles/accordion").Include(
                "~/Scripts/accordion.js"));
            
            // These scripts are all used on the GetHealthStatus View only.
            bundles.Add(new ScriptBundle("~/bundles/ajax").Include(
                "~/Scripts/refresh.js",
                "~/Scripts/displayloadingimage.js",
                "~/Scripts/displaystatus.js",
                "~/Scripts/homescroll.js"));

            // This script is on the layout page.
            // Although there is only one script in this bundle there are still advantages to bundling it, such as minification, etc.
            bundles.Add(new ScriptBundle("~/bundles/jswarning").Include(
               "~/Scripts/hidejsnotpresent.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}