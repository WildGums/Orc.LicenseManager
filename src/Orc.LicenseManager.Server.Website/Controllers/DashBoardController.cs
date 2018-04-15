// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DashBoardController.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Data.Entity;

    [Authorize(Roles = "Admin")]
    public class DashboardController : ApiController
    {
        #region Fields
        LicenseManagerDbContext db = new LicenseManagerDbContext();
        #endregion

        #region Constructors
        public DashboardController()
        {
        }
        #endregion

        #region Methods
        [ActionName("GetLast5Products")]
        public object GetLast5Products()
        {
            return db.Products.Include(x => x.Licenses).OrderByDescending(x => x.CreationDate).Take(5).Select(x => new
            {
                x.Id,
                Name = x.Name
            });
        }

        [ActionName("GetLast5Customers")]
        public object GetLast5Customers()
        {

            return db.Customers.Include(x => x.Licenses).OrderByDescending(x => x.CreationDate).Take(5).Select(x => new
            {
                x.Id,
                FullName = x.FirstName + " " + x.LastName
            });
        }

        [ActionName("GetLast5Licenses")]
        public object GetLast5Licenses()
        {
            var licenses = db.Licenses.Include(x => x.Customer).Include(x => x.Product).OrderByDescending(x => x.CreationDate).Take(5).Select(x => new
            {
                x.Id,
                Product = new
                {
                    Id = x.ProductId,
                    Name = x.Product.Name

                },
                Customer = new
                {
                    Id = x.CustomerId,
                    Name = x.Customer.FirstName + " " + x.Customer.LastName
                }
            });
            return licenses;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        #endregion
    }
}