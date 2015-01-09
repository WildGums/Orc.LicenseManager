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
    }
}