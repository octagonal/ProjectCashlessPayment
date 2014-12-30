using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace nmct.ba.cashlessproject.web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Organisation_Register",
                "Organisation_Register/{action}/{orgid}/{regid}",
                new { controller = "Organisation_RegisterController", action = "Index", orgid = UrlParameter.Optional, regid = UrlParameter.Optional }
            );
        }
    }
}
