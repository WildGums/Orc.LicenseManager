// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using Microsoft.Owin;

[assembly: OwinStartup(typeof (Orc.LicenseManager.Server.Website.Startup))]

namespace Orc.LicenseManager.Server.Website
{
    using Owin;

    public partial class Startup
    {
        #region Methods
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
        #endregion
    }
}