// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website
{
    using System.Data.Entity;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Catel.IoC;
    using Newtonsoft.Json;
    using Server.Services;
    using Services;

    public class MvcApplication : HttpApplication
    {
        #region Methods
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Catel.Mvc.DependencyInjectionConfig.RegisterServiceLocatorAsDependencyResolver();

            var serviceLocator = ServiceLocator.Default;
            serviceLocator.RegisterType<IMembershipService, MembershipService>();
            serviceLocator.RegisterType<ILicenseService, LicenseService>();

            // http://stackoverflow.com/questions/7397207/json-net-error-self-referencing-loop-detected-for-type
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };
        }
        #endregion
    }
}