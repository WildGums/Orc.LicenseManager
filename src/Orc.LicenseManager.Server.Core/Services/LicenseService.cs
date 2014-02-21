// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Services
{
    using System;
    using Portable.Licensing;

    public class LicenseService : ILicenseService
    {
        #region ILicenseService Members
        public LicensePoco GenerateLicenseForProduct(Product product)
        {
            var licensepoco = new LicensePoco();
            var license = License.New()
                .WithUniqueIdentifier(Guid.NewGuid())
                .As(LicenseType.Standard)
                .CreateAndSignWithPrivateKey(product.PrivateKey, product.PassPhrase);
            licensepoco.Value = license.ToString();
            return licensepoco;
        }

        #endregion

        #region Methods
        public Product GenerateKeysForProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.PassPhrase))
            {
                throw new Exception("No passphrase was given. Cannot generate keys.");
            }
            var keyGenerator = Portable.Licensing.Security.Cryptography.KeyGenerator.Create();
            var keyPair = keyGenerator.GenerateKeyPair();
            product.PrivateKey = keyPair.ToEncryptedPrivateKeyString(product.PassPhrase);
            product.PublicKey = keyPair.ToPublicKeyString();
            return product;
        }
        #endregion
    }
}