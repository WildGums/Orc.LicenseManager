using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orc.LicenseManager.Server.Data
{
    using System.Data.Entity;
    using Catel.IoC;
    using MaxBox.Core.Services;
    using Services;
    public class RecreateContext : DropCreateDatabaseAlways<LicenseManagerDbContext>
    {
        private readonly IAccountService _accountService;
        private readonly ILicenseService _licenseService;
        private readonly IRngService _stringService;

        public RecreateContext(IAccountService accountService, ILicenseService licenseService, IRngService stringService)
        {
            _accountService = accountService;
            _licenseService = licenseService;
            _stringService = stringService;
        }

        protected override void Seed(LicenseManagerDbContext context)
        {


            _accountService.CreateRole("Admin");
            _accountService.CreateUserWithRoles("Maxim", "password123", new List<string>() { "Admin" });
            _accountService.CreateUserWithRoles("Geert", "password123", new List<string>() { "Admin" });
            var maximId = context.Users.First(x => x.UserName == "Maxim").Id;

            using (var uow = new UoW())
            {
                for (int x = 1; x < 20; x++)
                {
                    var product = new Product();
                    product.Name = _stringService.GenerateString(7, true, false);
                    product.CreatorId = maximId;
                    _licenseService.GeneratePassPhraseForProduct(product);
                    _licenseService.GenerateKeysForProduct(product);
                    uow.Products.Add(product);
                    var customer = new Customer();
                    customer.FirstName = _stringService.GenerateString(7, true, false);
                    customer.LastName = _stringService.GenerateString(7, true, false);
                    customer.CreatorId = maximId;
                    uow.Customers.Add(customer);
                }
                uow.SaveChanges();
                    
            }
            base.Seed(context);
        }
        
    }
}
