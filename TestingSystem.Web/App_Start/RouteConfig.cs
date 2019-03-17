using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestingSystem.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "UserCreate",
                url: "User/Create/{groupId}/{specId}",
                defaults: new { controller = "User", action = "Create", groupId = 0 , specId = 0}
            );

            routes.MapRoute(
                name: "UserEdit",
                url: "User/Edit/{id}",
                defaults: new { controller = "User", action = "Edit", id = 0 }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
