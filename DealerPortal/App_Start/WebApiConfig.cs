using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DealerPortal {

    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                  routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes
             .Add(new MediaTypeHeaderValue("text/html"));
        }
    }

}
