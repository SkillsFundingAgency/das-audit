﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using SFA.DAS.Audit.Infrastructure.Logging;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure;

namespace SFA.DAS.Audit.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            LoggingConfig.ConfigureLogging();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            TelemetryConfiguration.Active.InstrumentationKey = CloudConfigurationManager.GetSetting("InstrumentationKey");
        }
    }
}