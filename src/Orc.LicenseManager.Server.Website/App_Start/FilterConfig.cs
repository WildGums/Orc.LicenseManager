using System.Web;
using System.Web.Mvc;

namespace Orc.LicenseManager.Server.Website
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
