using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace fastforward
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
                name: "Survey",
                url: "Home/Survey/"
                );

            routes.MapRoute(
                name: "Result",
                url: "Home/Result/"
            );

            routes.MapRoute(
               name: "Timeline",
               url: "Home/Timeline/"
            );

        }
    }
}