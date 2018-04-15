// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website.Controllers
{
    using System.Data.Entity;
    using System.Web.Mvc;
    using Catel.IoC;
    using Data;

    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

#if DEBUG
        [AllowAnonymous]
        public ActionResult Reset()
        {
            Database.SetInitializer<LicenseManagerDbContext>(TypeFactory.Default.CreateInstance<RecreateContext>());

            using (var context = new LicenseManagerDbContext())
            {
                context.Database.Initialize(true);
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Update()
        {
            Database.SetInitializer<LicenseManagerDbContext>(null);

            using (var context = new LicenseManagerDbContext())
            {
                context.Database.Initialize(true);
            }

            return RedirectToAction("Index", "Home");
        }
#endif
    }
}