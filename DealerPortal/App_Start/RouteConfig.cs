using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DealerPortal {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "StockAdjustmentSalesReturn",
            url: "SalesReturn",
            defaults: new
            {
                controller = "Home",
                action = "SalesReturn",
            }
            );


            routes.MapRoute(
            name: "PurchaseReturn",
            url: "PurchaseReturn",
            defaults: new
            {
                controller = "Home",
                action = "PurchaseReturn",
            }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
