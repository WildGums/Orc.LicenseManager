// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseGenerationService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml;
    using Catel.Data;
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
        /// Initializes the specified application identifier.
        /// </summary>
        /// <param name="applicationId">The application identifier.</param>
        /// <exception cref="ArgumentException">The <paramref name="applicationId" /> is <c>null</c> or whitespace.</exception>
        Task Initialize(string applicationId);

        /// <summary>
        /// Validates the license.
        /// </summary>
        /// <param name="license">The license key the user has given to be validated.</param>
        /// <returns>
        /// The validation context containing all the validation results.
        /// </returns>
        /// <exception cref="System.Exception">Please use the Initialize method first</exception>
        /// <exception cref="Exception">The <see cref="LicenseService.Initialize" /> method must be run first.</exception>
        IValidationContext ValidateLicense(string license);

        /// <summary>
        /// Saves the license.
        /// </summary>
        /// <param name="license">The license key that will be saved to <c>Catel.IO.Path.GetApplicationDataDirectory</c> .</param>
        /// <returns>Returns only true if the license is valid.</returns>
        /// <exception cref="ArgumentException">The <paramref name="license" /> is <c>null</c> or whitespace.</exception>
        void SaveLicense(string license);

        /// <summary>
        /// Removes the license if exists.
        /// </summary>
        void RemoveLicense();

        /// <summary>
        /// Check if the license exists.
        /// </summary>
        /// <returns>returns <c>true</c> if exists else <c>false</c></returns>
        bool LicenseExists();

        /// <summary>
        /// Loads the license.
        /// </summary>
        /// <returns>The license from <c>Catel.IO.Path.GetApplicationDataDirectory</c> unless it failed to load then it returns an empty string</returns>
        string LoadLicense();

        /// <summary>
        /// Validates the XML
        /// </summary>
        /// <param name="license">The license.</param>
        /// <returns>The validation context containing all the validation results.</returns>
        /// <exception cref="ArgumentException">The <paramref name="license" /> is <c>null</c> or whitespace.</exception>
        /// <exception cref="XmlException">The license text is not valid XML.</exception>
        /// <exception cref="Exception">The root element is not License.</exception>
        /// <exception cref="Exception">There were no inner nodes found.</exception>
        IValidationContext ValidateXml(string license);

        /// <summary>
        /// Loads the XML out of license.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <returns>
        /// A List of with the xml names and values
        /// </returns>
        List<XmlDataModel> LoadXmlFromLicense(string license);

        /// <summary>
        /// Validates the license on the server.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <param name="serverUrl">The server URL.</param>
        /// <param name="assembly">The assembly to get the information from. If <c>null</c>, the entry assembly will be used.</param>
        /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
        Task<LicenseValidationResult> ValidateLicenseOnServer(string license, string serverUrl, Assembly assembly = null);
    }
}