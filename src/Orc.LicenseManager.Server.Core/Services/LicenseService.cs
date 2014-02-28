// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Services
{
    using System;
    using System.Collections.Generic;
    using Catel.IoC;
    using MaxBox.Core.Services;
    using Portable.Licensing;

    public class LicenseService : ILicenseService
    {
        #region ILicenseService Members
        public void GenerateLicenseValue(LicensePoco licensepoco)
        {
            var tempProduct = licensepoco.Product;
            if (tempProduct == null)
            {
                using (var uow = new UoW())
                {
                    tempProduct = uow.Products.GetByKey(licensepoco.ProductId);
                }
            }
            var license = License.New();
            if (licensepoco.ExpireDate != null)
            {
                license = license.ExpiresAt((DateTime)licensepoco.ExpireDate);
            }
            if (licensepoco.ExpireVersion != null)
            {
                license = license.WithProductFeatures(new Dictionary<string, string>
                {
                    {"Version", licensepoco.ExpireVersion.ToString()}
                });
            }
            var finalLicense = license.WithUniqueIdentifier(Guid.NewGuid())
                .As(LicenseType.Standard)
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
            var stringService = ServiceLocator.Default.ResolveType<IStringService>();
            product.PassPhrase = stringService.GenerateString(15);
        }
        #endregion
    }
}