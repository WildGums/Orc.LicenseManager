using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Orc.LicenseManager.Server;

namespace Orc.LicenseManager.Server.Website.Controllers
{
    using System.Web.UI.WebControls;
    using Catel.IoC;
    using Services;

    public class LicenseController : Controller
    {
        private LicenseManagerDbContext db = new LicenseManagerDbContext();

        // GET: /License/
        public ActionResult Index()
        {
            var licenses = db.Licenses.Include(l => l.Creator);
            return View(licenses.ToList());
        }

        // GET: /License/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicensePoco licensepoco = db.Licenses.Find(id);
            if (licensepoco == null)
            {
                return HttpNotFound();
            }
            return View(licensepoco);
        }

        // GET: /License/Create
        public ActionResult Create()
        {
            ViewBag.CreatorId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: /License/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Comment")] LicensePoco licensepoco)
        {
            licensepoco.Creator = 
            if (ModelState.IsValid)
            {
                var licenseservice = ServiceLocator.Default.ResolveType<ILicenseService>();
                licenseservice.GenerateLicenseForProduct(licensepoco);
                db.Licenses.Add(licensepoco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatorId = new SelectList(db.Users, "Id", "UserName", licensepoco.CreatorId);
            return View(licensepoco);
        }

        // GET: /License/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicensePoco licensepoco = db.Licenses.Find(id);
            if (licensepoco == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "UserName", licensepoco.CreatorId);
            return View(licensepoco);
        }

        // POST: /License/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Value,Comment,CreatorId,ModificationDate,CreationDate")] LicensePoco licensepoco)
        {
            if (ModelState.IsValid)
            {
                db.Entry(licensepoco).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "UserName", licensepoco.CreatorId);
            return View(licensepoco);
        }

        // GET: /License/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicensePoco licensepoco = db.Licenses.Find(id);
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
            LicensePoco licensepoco = db.Licenses.Find(id);
            db.Licenses.Remove(licensepoco);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
