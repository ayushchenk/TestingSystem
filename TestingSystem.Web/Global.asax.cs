﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TestingSystem.Web.Infrastructure;
using TestingSystem.Web.Infrastructure.Autofac;
using TestingSystem.Web.Models.ViewModels;
namespace TestingSystem.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutofacConfig.ConfigureContainer();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.MessageHandlers.Add(new AuthorizationHeaderHandler());

            System.Web.Optimization.BundleTable.EnableOptimizations = true;
        }
    }
}