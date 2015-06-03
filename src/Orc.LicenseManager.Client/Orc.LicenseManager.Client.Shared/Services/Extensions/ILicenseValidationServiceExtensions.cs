// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseValidationServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System.Reflection;
    using System.Threading.Tasks;
    using Catel.Threading;
    using Models;

    public static class ILicenseValidationServiceExtensions
    {
        public static Task<LicenseValidationResult> ValidateLicenseOnServer(this ILicenseValidationService licenseValidationService, string license, string serverUrl, Assembly assembly = null)
        {
            return TaskHelper.Run(() => licenseValidationService.ValidateLicenseOnServer(license, serverUrl, assembly));
        }
    }
}