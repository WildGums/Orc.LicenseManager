using System.Web;
using System.Web.Mvc;

namespace Orc.LicenseManager.Server.Webservice
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}