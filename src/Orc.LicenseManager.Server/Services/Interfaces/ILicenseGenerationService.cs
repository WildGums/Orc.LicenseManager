// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Services
{
    public interface ILicenseGenerationService
    {
        #region Methods
        void GenerateLicenseValue(LicensePoco license);
        void GenerateKeysForProduct(Product product);
        void GeneratePassPhraseForProduct(Product product);
        #endregion
    }
}