

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
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
		
        // GET: /Customer/
        public ActionResult Index()
        {
			Log.Debug("GET/Index");
			using(var uow = new UoW()){
				var customersRepo = uow.GetRepository<ICustomerRepository>();
				var customers = customersRepo.GetQuery().Include(c => c.Creator).ToList();
				return View(customers);
			}
        }

        // GET: /Customer/Details/5
        public ActionResult Details(int? id)
        {
			Log.Debug("GET/Details id: {0}", id.ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = null;
			using(var uow = new UoW()){
				var customersRepo = uow.GetRepository<ICustomerRepository>();
				customer = customersRepo.GetByKey((System.Int32)id);
			}
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: /Customer/Create
        public ActionResult Create()
        {
			Log.Debug("GET/Create");
			using(var uow = new UoW()){
                //ViewBag.CreatorId = new SelectList(uow.IdentityUsers.GetAll(), "Id", "UserName");
			}
            return View();
        }

        // POST: /Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,FirstName,LastName,Company,Email,City,Postal,Street,Country,CreatorId,ModificationDate,CreationDate")] Customer customer)
        {
			Log.Debug("POST/Create");
            if (ModelState.IsValid)
            {
				using(var uow = new UoW()){
					var customersRepo = uow.GetRepository<ICustomerRepository>();
					customersRepo.Add(customer);
					uow.SaveChanges();
				}
                return RedirectToAction("Index");
            }

			using(var uow = new UoW()){
                //ViewBag.CreatorId = new SelectList(uow.IdentityUsers.GetAll(), "Id", "UserName");
			}
            return View(customer);
        }

        // GET: /Customer/Edit/5
        public ActionResult Edit(int? id)
        {
			Log.Debug("GET/Edit id:{0}", id.ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = null;
			using(var uow = new UoW()){
				var customersRepo = uow.GetRepository<ICustomerRepository>();
				customer = customersRepo.GetByKey((System.Int32)id);
			}
            if (customer == null)
            {
                return HttpNotFound();
            }
			using(var uow = new UoW()){
                //ViewBag.CreatorId = new SelectList(uow.IdentityUsers.GetAll(), "Id", "UserName");
			}
            return View(customer);
        }

        // POST: /Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,FirstName,LastName,Company,Email,City,Postal,Street,Country,CreatorId,ModificationDate,CreationDate")] Customer customer)
        {
			Log.Debug("POST/Edit");
            if (ModelState.IsValid)
            {
				using(var uow = new UoW()){
					var customersRepo = uow.GetRepository<ICustomerRepository>();
					
					customersRepo.Update(customer);
					uow.SaveChanges();
				}
			}	
			using(var uow = new UoW()){
                //ViewBag.CreatorId = new SelectList(uow.IdentityUsers.GetAll(), "Id", "UserName");
			}
            return View(customer);
        }

        // GET: /Customer/Delete/5
        public ActionResult Delete(int? id)
        {
		Log.Debug("GET/Delete Id:{0}, id.ToString()");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = null;
			using(var uow = new UoW()){
				var customersRepo = uow.GetRepository<ICustomerRepository>();
				customer = customersRepo.GetByKey((System.Int32)id);
			}
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: /Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			Log.Debug("POST/DeleteConfirmed Id:{0}", id.ToString());
			using(var uow = new UoW()){
				var customersRepo = uow.GetRepository<ICustomerRepository>();
				var customer = customersRepo.GetByKey((System.Int32)id);
				customersRepo.Delete(customer);
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
