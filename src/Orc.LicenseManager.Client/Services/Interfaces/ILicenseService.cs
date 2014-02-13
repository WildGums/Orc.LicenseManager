// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Catel.Data;
    using Catel.Fody;
    using Models;
    using Portable.Licensing.Validation;

    /// <summary>
    /// Service to validate, store and remove licenses for software products.
    /// </summary>
    public interface ILicenseService
    {
        /// <summary>
        /// Initializes the specified application identifier.
        /// </summary>
        /// <param name="applicationId">The application identifier.</param>
        /// <exception cref="ArgumentException">The <paramref name="applicationId" /> is <c>null</c> or whitespace.</exception>
        void Initialize([NotNullOrWhitespace] string applicationId);

        /// <summary>
        /// Shows the single license dialog.
        /// </summary>
        /// <param name="title">The title. If <c>null</c>, the title will be extracted from the entry assembly.</param>
        /// <param name="website">The website. If <c>null</c>, no website link will be displayed.</param>
        /// <exception cref="Exception">The <see cref="LicenseService.Initialize" /> method must be run first.</exception>
        void ShowSingleLicenseDialog(string title = null, string website = null);

        /// <summary>
        /// Validates the lisence.
        /// </summary>
        /// <param name="license">The lisence key the user has given to be validated.</param>
        /// <returns>
        /// The validation context containing all the validation results.
        /// </returns>
        /// <exception cref="System.Exception">Please use the Initialize method first</exception>
        /// <exception cref="Exception">The <see cref="LicenseService.Initialize" /> method must be run first.</exception>
        IValidationContext ValidateLicense(string license);

        /// <summary>
        /// Saves the license.
        /// </summary>
        /// <param name="license">The lisence key that will be saved to <c>Catel.IO.Path.GetApplicationDataDirectory</c> .</param>
        /// <returns>Returns only true if the license is valid.</returns>
        /// <exception cref="ArgumentException">The <paramref name="license" /> is <c>null</c> or whitespace.</exception>
        void SaveLicense([NotNullOrWhitespace] string license);

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
        /// <returns>The lisence from <c>Catel.IO.Path.GetApplicationDataDirectory</c> unless it failed to load then it returns an empty string</returns>
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
        IValidationContext ValidateXML(string license);

        /// <summary>
        /// Loads the XML out of lisence.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <returns>
        /// A List of with the xml names and values
        /// </returns>
        List<XMLDataModel> LoadXMLFromLisence(string license);
    }
}