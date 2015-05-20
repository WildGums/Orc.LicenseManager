// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseValidationServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System.Reflection;
    using System.Threading.Tasks;
    using Models;

    public static class ILicenseValidationServiceExtensions
    {
        public static async Task<LicenseValidationResult> ValidateLicenseOnServer(this ILicenseValidationService licenseValidationService, string license, string serverUrl, Assembly assembly = null)
        {
            return await Task.Factory.StartNew(() => licenseValidationService.ValidateLicenseOnServer(license, serverUrl, assembly));
        }
    }
}