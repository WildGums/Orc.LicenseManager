// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Services
{
    public interface ILicenseService
    {
        #region Methods
        void GenerateLicenseValue(LicensePoco license);
        void GenerateKeysForProduct(Product product);
        void GeneratePassPhraseForProduct(Product product);
        #endregion
    }
}