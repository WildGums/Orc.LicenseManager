// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Services
{
    using System;
    using System.Collections.Generic;
    using Portable.Licensing;

    public class LicenseGenerationGenerationService : ILicenseGenerationService
    {
        #region ILicenseGenerationService Members
        public void GenerateLicenseValue(LicensePoco licensepoco)
        {
            var tempProduct = licensepoco.Product;
            var tempCustomer = licensepoco.Customer;
            if (tempProduct is null)
            {
                using (var uow = new UoW())
                {
                    tempProduct = uow.Products.GetByKey(licensepoco.ProductId);
                }
            }

            if (tempCustomer is null)
            {
                using (var uow = new UoW())
                {
                    tempCustomer = uow.Customers.GetByKey(licensepoco.CustomerId);
                }
            }

            var license = License.New();
            var productFeatures = new Dictionary<string, string>
            {
                {"FirstName", tempCustomer.FirstName},
                {"LastName", tempCustomer.LastName},
                {"Email", tempCustomer.Email}
            };

            if (licensepoco.ExpireDate is not null)
            {
                license = license.ExpiresAt((DateTime) licensepoco.ExpireDate);
            }

            if (licensepoco.ExpireVersion is not null)
            {
                productFeatures.Add("Version", licensepoco.ExpireVersion.ToString());
            }

            var finalLicense = license.WithUniqueIdentifier(Guid.NewGuid())
                .As(LicenseType.Standard)
                .WithMaximumUtilization(1)
                .WithProductFeatures(productFeatures)
                .CreateAndSignWithPrivateKey(tempProduct.PrivateKey, tempProduct.PassPhrase);

            licensepoco.Value = finalLicense.ToString();
        }

        public void GenerateKeysForProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.PassPhrase))
            {
                throw new Exception("No passphrase was given. Cannot generate keys.");
            }

            var keyGenerator = Portable.Licensing.Security.Cryptography.KeyGenerator.Create();
            var keyPair = keyGenerator.GenerateKeyPair();

            product.PrivateKey = keyPair.ToEncryptedPrivateKeyString(product.PassPhrase);
            product.PublicKey = keyPair.ToPublicKeyString();
        }

        public void GeneratePassPhraseForProduct(Product product)
        {
            //var stringService = ServiceLocator.Default.ResolveType<IRngService>();
            //product.PassPhrase = stringService.GenerateString(15);

            product.PassPhrase = Guid.NewGuid().ToString();
        }
        #endregion
    }
}