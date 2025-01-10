using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StoreOrderingDashBoard
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Store", action = "LogIn", id = UrlParameter.Optional }
            );

            // routes.MapRoute(
            //    name: "Admin_default",
            //    url: "Home/{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "LogIn", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
    "Home",
    "Home/{controller}/{action}/{id}",
    new { controller = "Home", action = "LogIn", id = UrlParameter.Optional },
    new[] { "StoreOrderingDashBoard.Controllers" });
        }
    }
}
