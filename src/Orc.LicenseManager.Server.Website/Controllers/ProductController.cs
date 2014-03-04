// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductController.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Catel.Logging;
    using Newtonsoft.Json;
    using Repositories;
    using Server.Services;

    //This controller template is created by Crabbé Maxim
    //This is a modified controller template that uses the Unit of Work pattern with the help
    //of Catel.Core and Catel.Extensions.EntityFramework
    //For more info about Catel visit http://www.catelproject.com
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly ILicenseService _licenseService;
        #endregion

        #region Constructors
        public ProductController(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }
        #endregion

        // GET: /Product/

        #region Methods
        public ActionResult Index()
        {
            Log.Debug("GET/Index");
            using (var uow = new UoW())
            {
                var productsRepo = uow.GetRepository<IProductRepository>();
                var products = productsRepo.GetQuery().Include(p => p.Creator).ToList();
                return View("Index", (object)JsonConvert.SerializeObject(products));
            }
        }

        // GET: /Product/Details/5
        public ActionResult Details(int? id)
        {
            Log.Debug("GET/Details id: {0}", id.ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = null;
            using (var uow = new UoW())
            {
                var productsRepo = uow.GetRepository<IProductRepository>();
                product = productsRepo.GetByKey((System.Int32) id);
            }
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: /Product/Create
        public ActionResult Create()
        {
            Log.Debug("GET/Create");
            return View();
        }

        // POST: /Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PassPhrase,PrivateKey,PublicKey,CreationDate,CreatorId")] Product product)
        {
            Log.Debug("POST/Create");
            if (ModelState.IsValid)
            {
                using (var uow = new UoW())
                {
                    var productsRepo = uow.GetRepository<IProductRepository>();
                    _licenseService.GeneratePassPhraseForProduct(product);
                    _licenseService.GenerateKeysForProduct(product);
                    productsRepo.Add(product);
                    uow.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: /Product/Edit/5
        public ActionResult Edit(int? id)
        {
            Log.Debug("GET/Edit id:{0}", id.ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = null;
            using (var uow = new UoW())
            {
                var productsRepo = uow.GetRepository<IProductRepository>();
                product = productsRepo.GetByKey((System.Int32) id);
            }
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: /Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PassPhrase,PrivateKey,PublicKey,CreationDate,CreatorId")] Product product)
        {
            Log.Debug("POST/Edit");
            if (ModelState.IsValid)
            {
                using (var uow = new UoW())
                {
                    var productsRepo = uow.GetRepository<IProductRepository>();

                    productsRepo.Update(product);
                    uow.SaveChanges();
                }
            }
            return View(product);
        }

        // GET: /Product/Delete/5
        public ActionResult Delete(int? id)
        {
            Log.Debug("GET/Delete Id:{0}, id.ToString()");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = null;
            using (var uow = new UoW())
            {
                var productsRepo = uow.GetRepository<IProductRepository>();
                product = productsRepo.GetByKey((System.Int32) id);
            }
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: /Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Log.Debug("POST/DeleteConfirmed Id:{0}", id.ToString());
            using (var uow = new UoW())
            {
                var productsRepo = uow.GetRepository<IProductRepository>();
                var product = productsRepo.GetByKey((System.Int32) id);
                productsRepo.Delete(product);
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