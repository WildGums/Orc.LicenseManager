

namespace Orc.LicenseManager.Server.Website.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Entity;
	using System.Linq;
	using System.Net;
	using System.Web;
	using System.Web.Mvc;
	
	using Catel.Logging;
	
	using Repositories;

	using Orc.LicenseManager.Server;
	//This controller template is created by Crabbé Maxim
	//This is a modified controller template that uses the Unit of Work pattern with the help
	//of Catel.Core and Catel.Extensions.EntityFramework
	//For more info about Catel visit http://www.catelproject.com
    public class LicenseController : Controller
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
		
        // GET: /License/
        public ActionResult Index()
        {
			Log.Debug("GET/Index");
			using(var uow = new UoW()){
				var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
				var licenses = licensesRepo.GetQuery().Include(l => l.Creator).Include(l => l.Customer).Include(l => l.Product).ToList();
				return View(licenses);
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
			using(var uow = new UoW()){
				var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
				licensepoco = licensesRepo.GetByKey((System.Int32)id);
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
			using(var uow = new UoW()){
				ViewBag.CustomerId = new SelectList(uow.Customers.GetAll(), "Id", "FirstName");
				ViewBag.ProductId = new SelectList(uow.Products.GetAll(), "Id", "Name");
			}
            return View();
        }

        // POST: /License/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Value,CustomerId,ProductId,CreatorId,ModificationDate,CreationDate")] LicensePoco licensepoco)
        {
			Log.Debug("POST/Create");
            if (ModelState.IsValid)
            {
				using(var uow = new UoW()){
					var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
					licensesRepo.Add(licensepoco);
					uow.SaveChanges();
				}
                return RedirectToAction("Index");
            }

			using(var uow = new UoW()){
				ViewBag.CustomerId = new SelectList(uow.Customers.GetAll(), "Id", "FirstName");
				ViewBag.ProductId = new SelectList(uow.Products.GetAll(), "Id", "Name");
			}
            return View(licensepoco);
        }

        // GET: /License/Edit/5
        public ActionResult Edit(int? id)
        {
			Log.Debug("GET/Edit id:{0}", id.ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicensePoco licensepoco = null;
			using(var uow = new UoW()){
				var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
				licensepoco = licensesRepo.GetByKey((System.Int32)id);
			}
            if (licensepoco == null)
            {
                return HttpNotFound();
            }
			using(var uow = new UoW()){
				ViewBag.CustomerId = new SelectList(uow.Customers.GetAll(), "Id", "FirstName");
				ViewBag.ProductId = new SelectList(uow.Products.GetAll(), "Id", "Name");
			}
            return View(licensepoco);
        }

        // POST: /License/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Value,CustomerId,ProductId,CreatorId,ModificationDate,CreationDate")] LicensePoco licensepoco)
        {
			Log.Debug("POST/Edit");
            if (ModelState.IsValid)
            {
				using(var uow = new UoW()){
					var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
					
					licensesRepo.Update(licensepoco);
					uow.SaveChanges();
				}
			}	
			using(var uow = new UoW()){
				ViewBag.CustomerId = new SelectList(uow.Customers.GetAll(), "Id", "FirstName");
				ViewBag.ProductId = new SelectList(uow.Products.GetAll(), "Id", "Name");
			}
            return View(licensepoco);
        }

        // GET: /License/Delete/5
        public ActionResult Delete(int? id)
        {
		Log.Debug("GET/Delete Id:{0}, id.ToString()");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicensePoco licensepoco = null;
			using(var uow = new UoW()){
				var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
				licensepoco = licensesRepo.GetByKey((System.Int32)id);
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
			using(var uow = new UoW()){
				var licensesRepo = uow.GetRepository<ILicensePocoRepository>();
				var licensepoco = licensesRepo.GetByKey((System.Int32)id);
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
    }
}
