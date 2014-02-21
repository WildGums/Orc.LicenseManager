using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Orc.LicenseManager.Server.Website.Startup))]
namespace Orc.LicenseManager.Server.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
