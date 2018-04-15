// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseGenerationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Portable.Licensing;

    /// <summary>
    /// Service to validate, store and remove licenses for software products.
    /// </summary>
    public interface ILicenseService
    {
        /// <summary>
        /// Gets the current license.
        /// </summary>
        /// <value>The current license.</value>
        License CurrentLicense { get; }

        /// <summary>
        /// Saves the license.
        /// </summary>
        /// <param name="license">The license key that will be saved to <c>Catel.IO.Path.GetApplicationDataDirectory</c> .</param>
        /// <param name="licenseMode"></param>
        /// <returns>Returns only true if the license is valid.</returns>
        /// <exception cref="ArgumentException">The <paramref name="license" /> is <c>null</c> or whitespace.</exception>
        void SaveLicense(string license, LicenseMode licenseMode = LicenseMode.CurrentUser);

        /// <summary>
        /// Removes the license if exists.
        /// </summary>
        /// <param name="licenseMode"></param>
        void RemoveLicense(LicenseMode licenseMode = LicenseMode.CurrentUser);

        /// <summary>
        /// Check if the license exists.
        /// </summary>
        /// <returns>returns <c>true</c> if exists else <c>false</c></returns>
        bool LicenseExists(LicenseMode licenseMode = LicenseMode.CurrentUser);

        /// <summary>
        /// Loads the license.
        /// </summary>
        /// <returns>The license from <c>Catel.IO.Path.GetApplicationDataDirectory</c> unless it failed to load then it returns an empty string</returns>
        string LoadLicense(LicenseMode licenseMode = LicenseMode.CurrentUser);

        /// <summary>
        /// Loads the XML out of license.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <returns>
        /// A List of with the xml names and values
        /// </returns>
        List<XmlDataModel> LoadXmlFromLicense(string license);
    }
}