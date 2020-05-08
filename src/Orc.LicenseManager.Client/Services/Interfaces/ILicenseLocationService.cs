// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseLocationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
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
