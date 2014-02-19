// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LicenseManager.Server.Website.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        #region Methods
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
        #endregion
    }
}