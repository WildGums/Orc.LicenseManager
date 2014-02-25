using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orc.LicenseManager.Server.Data
{
    using System.Data.Entity;
    using Catel.IoC;
    using Services;

    public class RecreateContext : DropCreateDatabaseAlways<LicenseManagerDbContext>
    {
        private readonly IAccountService _accountService;
        private readonly ILicenseService _licenseService;

        public RecreateContext(IAccountService accountService, ILicenseService licenseService)
        {
            _accountService = accountService;
            _licenseService = licenseService;
        }

        protected override void Seed(LicenseManagerDbContext context)
        {

            _accountService.CreateRole("Admin");
            _accountService.CreateUserWithRoles("Maxim", "password123", new List<string>() { "Admin" });
            _accountService.CreateUserWithRoles("Geert", "password123", new List<string>() { "Admin" });
            var maximId = context.Users.First(x => x.UserName == "Maxim").Id;

            using (var uow = new UoW())
            {
                for (int x = 1; x < 6; x++)
                {
                    var product = new Product();
                    product.Name = "Product" + x;
                    product.CreatorId = maximId;
                    _licenseService.GeneratePassPhraseForProduct(product);
                    _licenseService.GenerateKeysForProduct(product);
                    uow.Products.Add(product);
                    var customer = new Customer();
                    customer.FirstName = "Cust" + x;
                    customer.LastName = "Emer" + x;
                    customer.CreatorId = maximId;
                    uow.Customers.Add(customer);
                }
                uow.SaveChanges();
                    
            }
            base.Seed(context);
        }
        
    }
}
