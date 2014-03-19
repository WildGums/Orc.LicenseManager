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

    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            //// heb dit hier gezet voor te testen
            //var context = new LicenseManagerDbContext();
            //var alluserscontext = context.Users.ToList(); // werkt wel

            //using (var uow = new UoW())
            //{
            //    var repository = uow.GetRepository<IUserRepository>();
            //    repository.GetQuery().ToList(); // crash 
            //} // hij geraakt hier niet 
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
    }
}