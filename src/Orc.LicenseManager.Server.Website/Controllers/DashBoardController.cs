// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DashBoardController.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Catel.Data;

    public class DashboardController : ApiController
    {
        #region Fields
        private readonly LicenseManagerDbContext dbContext = new LicenseManagerDbContext();
        #endregion

        #region Constructors
        public DashboardController()
        {
            dbContext.Configuration.ProxyCreationEnabled = true;
        }
        #endregion

        #region Methods
        [ActionName("GetLast5Products")]
        public IEnumerable<Product> GetLast5Products()
        {
            using (var db = dbContext)
            {
                var products = db.Products.Include(x => x.Licenses).OrderByDescending(x => x.CreationDate).Take(5);
                return products.ToList();
            }
        }

        [ActionName("GetLast5Customers")]
        public IEnumerable<Customer> GetLast5Customers()
        {
            using (var db = dbContext)
            {
                var customers = db.Customers.Include(x => x.Licenses).OrderByDescending(x => x.CreationDate).Take(5);
                return customers.ToList();
            }
        }

        [ActionName("GetLast5Licenses")]
        public object GetLast5Licenses()
        {
            using (var db = dbContext)
            {
                var licenses = db.Licenses.Include(x=>x.Customer).Include(x=>x.Product).OrderByDescending(x => x.CreationDate).Take(5).Select(x => new 
                {
                    x.Id
                    //,ProductName = new
                    //{
                    //    x.Product.Name
                    //},
                    //CustomerName =x.Customer.FirstName + " " + x.Customer.LastName

                });;
               //var licenseobject =  licenses
                return licenses;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion
    }
}