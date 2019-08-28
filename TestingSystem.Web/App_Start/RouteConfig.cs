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
                name: "StudentContent",
                url: "StudentContent/{action}/{id}",
                defaults: new { controller = "StudentContent", action = "Tests", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "TeacherContent",
                url: "TeacherContent/{action}/{id}",
                defaults: new { controller = "TeacherContent", action = "Welcome", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Education", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}