using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orc.LicenseManager.Server.Website.Controllers
{
    using System.Data.Entity;
    using Catel.IoC;
    using Data;
    using Repositories;

    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
#if DEBUG
     
        public ActionResult Reset()
        {
            Database.SetInitializer<LicenseManagerDbContext>(TypeFactory.Default.CreateInstance<RecreateContext>());
            using (var context = new LicenseManagerDbContext())
            {
                context.Database.Initialize(true);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Update()
        {
            Database.SetInitializer<LicenseManagerDbContext>(null);
            var context = new LicenseManagerDbContext();
            context.Database.Initialize(true);
            return RedirectToAction("Index", "Home");
        }
#endif 
    }
}