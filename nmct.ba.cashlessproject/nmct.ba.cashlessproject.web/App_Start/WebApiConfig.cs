using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace nmct.ba.cashlessproject.web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                    name: "SaleSpan",
                    routeTemplate: "api/{controller}/{type}/{id}/{periodStart}/{periodEnd}",
                    defaults: new { controller = "SaleController" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
