// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using Catel.Data;
    using Portable.Licensing.Validation;

    /// <summary>
    /// Service to validate, store and remove licenses for software products.
    /// </summary>
    public interface ILicenseService
    {
        #region Methods
        /// <summary>
        /// Initializes the specified application identifier.
        /// </summary>
        /// <param name="applicationId">The application identifier.</param>
        /// <exception cref="ArgumentException">The <paramref name="applicationId"/> is <c>null</c> or whitespace.</exception>
        void Initialize(string applicationId);

        /// <summary>
        /// Shows the single license dialog.
        /// </summary>
        /// <param name="title">The title. If <c>null</c>, the title will be extracted from the entry assembly.</param>
        /// <param name="website">The website.  If <c>null</c>, no website link will be displayed.</param>
        /// <exception cref="Exception">The  <see cref="Initialize"/> method must be run first.</exception>
        void ShowSingleLicenseDialog(string title = null, string website = null);

        /// <summary>
        /// Validates the lisence.
        /// </summary>
        /// <param name="license">The lisence key the user has given to be validated.</param>
        /// <returns>
        /// The validation context containing all the validation results.
        /// </returns>
        /// <exception cref="Exception">The <see cref="Initialize"/> method must be run first.</exception>
        IValidationContext ValidateLicense(string license);

        ///// <summary>
        ///// Gets the validation error.
        ///// </summary>
        ///// <returns>
        ///// An <c>IValidationFailure</c> if the validation failed.
        ///// </returns>
        ///// <exception cref="System.Exception">Please try to validate the lisence first.</exception>
        ///// <exception cref="Exception">The  <see cref="Initialize"/> method must be run first.</exception>
        //IValidationFailure GetValidationError();

        /// <summary>
        /// Saves the license.
        /// </summary>
        /// <param name="license">The lisence key that will be saved to <c>Catel.IO.Path.GetApplicationDataDirectory</c> .</param>
        /// <returns>Returns only true if the license is valid.</returns>
        /// <exception cref="ArgumentException">The <paramref name="license"/> is <c>null</c> or whitespace.</exception>
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
        /// <returns>The lisence from <c>Catel.IO.Path.GetApplicationDataDirectory</c> unless it failed to load then it returns an empty string</returns>
        string LoadLicense();
        #endregion
    }
}