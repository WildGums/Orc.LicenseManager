// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseServiceExtensions.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;
    using Catel;
    using Services;

    public static class ILicenseServiceExtensions
    {
        public static DateTime? GetCurrentLicenseExpirationDateTime(this ILicenseService licenseService)
        {
            Argument.IsNotNull(() => licenseService);

            var license = licenseService.CurrentLicense;
            return (license != null) ? license.Expiration : (DateTime?)null;
        }

        public static bool AnyLicenseExists(this ILicenseService licenseService)
        {
            return licenseService.LicenseExists(LicenseMode.CurrentUser) || licenseService.LicenseExists(LicenseMode.MachineWide);
        }

        public static string LoadExistedLicense(this ILicenseService licenseService)
        {
            var licenseString = licenseService.LoadLicense(LicenseMode.CurrentUser);
            if (string.IsNullOrWhiteSpace(licenseString))
            {
                licenseString = licenseService.LoadLicense(LicenseMode.MachineWide);
            }

            return licenseString;
        }
    }
}