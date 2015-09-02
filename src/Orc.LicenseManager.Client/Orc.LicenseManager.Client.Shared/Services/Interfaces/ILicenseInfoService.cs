// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseInfoService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using Models;

    public interface ILicenseInfoService
    {
        #region Methods
        LicenseInfo GetLicenseInfo();
        #endregion
    }
}