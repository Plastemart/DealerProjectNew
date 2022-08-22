using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace DealerPortal {

    public class BundleConfig {

        public static void RegisterBundles(BundleCollection bundles) {

            var scriptBundle = new ScriptBundle("~/Scripts/bundle");
            var styleBundle = new StyleBundle("~/Content/bundle");

            // jQuery
            scriptBundle
                .Include("~/Scripts/jquery-3.1.0.js");

            // Bootstrap
            scriptBundle
                .Include("~/Scripts/bootstrap.js");

            // For AutoComplete
            //scriptBundle
            //   .Include("~/Scripts/NewJS/assets/js/jquery-ui-1.8.24.custom.min.js");
            //scriptBundle
            //   .Include("~/Scripts/NewJS/assets/js/jquery.ui.core.js");
            //scriptBundle
            //   .Include("~/Scripts/NewJS/assets/js/jquery-ui.min.js");
            //scriptBundle
            //   .Include("~/Scripts/NewJS/assets/js/jquery.ui.autocomplete.js");
            //scriptBundle
            //  .Include("~/Scripts/zebra_datepicker.min.js");
            //End
            // Bootstrap
            styleBundle
                .Include("~/Content/bootstrap.css");

            // Custom site styles
            styleBundle
                .Include("~/Content/Site.css");

            // For AutoComplete
            //styleBundle
            //    .Include("~/Scripts/zebra_datepicker.css");
            //styleBundle
            //    .Include("~/Scripts/NewJS/assets/css/jquery-ui.css");
            // End

            bundles.Add(scriptBundle);
            bundles.Add(styleBundle);

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}