// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseLocationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    public interface ILicenseLocationService
    {
        /// <summary>
        /// Loads the license. Uses the <see cref="GetLicenseLocation"/> to retrieve the current location of the license.
        /// </summary>
        /// <param name="licenseMode">The license mode.</param>
        /// <returns>System.String.</returns>
        string LoadLicense(LicenseMode licenseMode);

        /// <summary>
        /// Gets the license information path.
        /// </summary>
        /// <param name="licenseMode">The license mode.</param>
        /// <returns>System.String.</returns>
        string GetLicenseLocation(LicenseMode licenseMode);
    }
}