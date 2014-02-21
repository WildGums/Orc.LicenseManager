using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orc.LicenseManager.Server.Website.Controllers
{
    using System.Data.Entity;
    using Data;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Reset()
        {
            Database.SetInitializer<LicenseManagerDbContext>(new RecreateContext());
            using (var context = new LicenseManagerDbContext())
            {
                context.Database.Initialize(true);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}