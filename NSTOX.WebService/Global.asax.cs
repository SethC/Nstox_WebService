﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.WindowsAzure;
using System.Configuration;
using System.Diagnostics;
using System.Web.Routing;
using System.Web.Mvc;

namespace NSTOX.WebService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                string connectionString;
                connectionString = ConfigurationManager.AppSettings[configName];
                configSetter(connectionString);
            });
            CloudTraceListener.StorageTraceListener.Instance.Write("Initialised");
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            RegisterRoutes(RouteTable.Routes);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Trace.WriteLine(Request.Url.ToString());
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            Trace.WriteLine(ex.ToString());
            Server.ClearError();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allsvc}", new { allsvc = @".*\.svc(/.*)?" });

            routes.MapRoute("Home", "", new { controller = "Home", action = "Ria" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional, option = UrlParameter.Optional } // Parameter defaults
            );
        }
    }
}