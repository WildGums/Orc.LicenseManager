// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
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
        void ShowSingleLicenseDialog(string title = null, string website = null);

        /// <summary>
        /// Validates the lisence.
        /// </summary>
        /// <param name="lisence">The lisence key the user has given to be validated.</param>
        /// <returns>Returns only true if the license is valid.</returns>
        /// <exception cref="ArgumentException">The <paramref name="lisence"/> is <c>null</c> or whitespace.</exception>
        bool ValidateLisence(string lisence);
        #endregion

        /// <summary>
        /// Gets the validation error.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Please try to validate the lisence first.</exception>
        IValidationFailure GetValidationError();
    }
}