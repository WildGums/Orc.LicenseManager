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
        LicensePoco GenerateLicenseForProduct(Product product);
        Product GenerateKeysForProduct(Product product);
        #endregion
    }
}