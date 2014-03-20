// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseController.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Catel.Data;
    using Catel.Logging;
    using Newtonsoft.Json;
    using Repositories;
    using Server.Services;

    //This controller template is created by Crabbé Maxim
    //This is a modified controller template that uses the Unit of Work pattern with the help
    //of Catel.Core and Catel.Extensions.EntityFramework
    //For more info about Catel visit http://www.catelproject.com
    public class LicenseController : BaseController
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly ILicenseService _licenseService;
        #endregion

        #region Constructors
        public LicenseController(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }
        #endregion

        // GET: /License/

        #region Methods
        public ActionResult Index()
        {
            Log.Debug("GET/Index");
            using (var uow = new UoW())
            {
                var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
                var licenses = licensesRepo.GetQuery().Include(l => l.Creator).Include(l => l.Customer).Include(l => l.Product).ToList();
                return View("Index", (object)JsonConvert.SerializeObject(licenses));

            }
        }

        // GET: /License/Details/5
        public ActionResult Details(int? id)
        {
            Log.Debug("GET/Details id: {0}", id.ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicensePoco licensepoco = null;
            using (var uow = new UoW())
            {
                var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
              //  licensepoco = licensesRepo.GetByKey((System.Int32) id); // Geert mayby add include?
                licensepoco = licensesRepo.GetQuery().Include(x=>x.Customer).Include(x=>x.Product).FirstOrDefault(x => x.Id == (System.Int32) id);
            }
            if (licensepoco == null)
            {
                return HttpNotFound();
            }
            return View(licensepoco);
        }

        // GET: /License/Create
        public ActionResult Create()
        {
            Log.Debug("GET/Create");
            using (var uow = new UoW())
            {
                ViewBag.CustomerId = JsonConvert.SerializeObject(uow.Customers.GetAll().ToList()); // new SelectList(uow.Customers.GetAll().ToList(), "Id", "FirstName");
                ViewBag.ProductId = JsonConvert.SerializeObject(uow.Products.GetAll().ToList());//new SelectList(uow.Products.GetAll().ToList(), "Id", "Name");
            }
            return View();
        }

        // POST: /License/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Value,ExpireVersion,ExpireDate,CustomerId,ProductId,CreatorId,ModificationDate,CreationDate")] LicensePoco licensepoco)
        {
            Log.Debug("POST/Create");
            if (ModelState.IsValid)
            {
                _licenseService.GenerateLicenseValue(licensepoco);
                using (var uow = new UoW())
                {
                    var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
                    licensesRepo.Add(licensepoco);
                    uow.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            using (var uow = new UoW())
            {
                ViewBag.CustomerId = JsonConvert.SerializeObject(uow.Customers.GetAll().ToList()); // new SelectList(uow.Customers.GetAll().ToList(), "Id", "FirstName");
                ViewBag.ProductId = JsonConvert.SerializeObject(uow.Products.GetAll().ToList());//new SelectList(uow.Products.GetAll().ToList(), "Id", "Name");
            }
            return View(licensepoco);
        }

        // GET: /License/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    Log.Debug("GET/Edit id:{0}", id.ToString());
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    LicensePoco licensepoco = null;
        //    using (var uow = new UoW())
        //    {
        //        var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
        //        licensepoco = licensesRepo.GetByKey((System.Int32) id);
        //    }
        //    if (licensepoco == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    using (var uow = new UoW())
        //    {
        //        ViewBag.CustomerId = new SelectList(uow.Customers.GetAll().ToList(), "Id", "FirstName");
        //        ViewBag.ProductId = new SelectList(uow.Products.GetAll().ToList(), "Id", "Name");
        //    }
        //    return View(licensepoco);
        //}

        //// POST: /License/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Value,ExpireVersion,ExpireDate,CustomerId,ProductId,CreatorId,ModificationDate,CreationDate")] LicensePoco licensepoco)
        //{
        //    Log.Debug("POST/Edit");
        //    if (ModelState.IsValid)
        //    {
        //        using (var uow = new UoW())
        //        {
        //            var licensesRepo = uow.GetRepository<ILicensePocoRepository>();

        //            licensesRepo.Update(licensepoco);
        //            uow.SaveChanges();
        //        }
        //    }
        //    using (var uow = new UoW())
        //    {
        //        ViewBag.CustomerId = new SelectList(uow.Customers.GetAll().ToList(), "Id", "FirstName");
        //        ViewBag.ProductId = new SelectList(uow.Products.GetAll().ToList(), "Id", "Name");
        //    }
        //    return View(licensepoco);
        //}

        // GET: /License/Delete/5
        public ActionResult Delete(int? id)
        {
            Log.Debug("GET/Delete Id:{0}, id.ToString()");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicensePoco licensepoco = null;
            using (var uow = new UoW())
            {
                var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
                licensepoco = licensesRepo.GetByKey((System.Int32) id);
            }
            if (licensepoco == null)
            {
                return HttpNotFound();
            }
            return View(licensepoco);
        }

        // POST: /License/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Log.Debug("POST/DeleteConfirmed Id:{0}", id.ToString());
            using (var uow = new UoW())
            {
                var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
                var licensepoco = licensesRepo.GetByKey((System.Int32) id);
                licensesRepo.Delete(licensepoco);
                uow.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //UoW dispose ofzo?
            base.Dispose(disposing);
        }
        #endregion
    }
}