// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecreateContext.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Data
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using Services;

    public class RecreateContext : DropCreateDatabaseAlways<LicenseManagerDbContext>
    {
        #region Fields
        private readonly IAccountService _accountService;
        private readonly ILicenseGenerationService _licenseGenerationService;
        #endregion

        #region Constructors
        public RecreateContext(IAccountService accountService, ILicenseGenerationService licenseGenerationService)
        {
            _accountService = accountService;
            _licenseGenerationService = licenseGenerationService;
        }
        #endregion

        #region Methods
        protected override void Seed(LicenseManagerDbContext context)
        {
            _accountService.CreateRole("Admin");
            _accountService.CreateUserWithRoles("Admin", "password123", new List<string>() {"Admin"});
            //_accountService.CreateUserWithRoles("Maxim", "password123", new List<string>() { "Admin" });
            //_accountService.CreateUserWithRoles("Geert", "password123", new List<string>() { "Admin" });
            //var maximId = context.Users.First(x => x.UserName == "Maxim").Id;

            //using (var uow = new UoW())
            //{
            //    for (int x = 1; x < 20; x++)
            //    {
            //        var product = new Product();
            //        product.Name = _stringService.GenerateString(7, true, false);
            //        product.CreatorId = maximId;
            //        _licenseGenerationService.GeneratePassPhraseForProduct(product);
            //        _licenseGenerationService.GenerateKeysForProduct(product);
            //        uow.Products.Add(product);
            //        var customer = new Customer();
            //        customer.FirstName = _stringService.GenerateString(7, true, false);
            //        customer.LastName = _stringService.GenerateString(7, true, false);
            //        customer.CreatorId = maximId;
            //        uow.Customers.Add(customer);
            //    }
            //    uow.SaveChanges();

            //}
            base.Seed(context);
        }
        #endregion
    }
}