// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseModeService.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System.Collections.Generic;

    public interface ILicenseModeService
    {
        List<LicenseMode> GetAvailableLicenseModes();
        bool IsLicenseModeAvailable(LicenseMode licenseMode);
    }
}