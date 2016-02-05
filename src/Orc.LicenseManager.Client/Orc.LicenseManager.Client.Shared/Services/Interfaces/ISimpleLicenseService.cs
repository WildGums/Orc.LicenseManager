// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISimpleLicenseService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    /// <summary>
    /// A very simple implementation of the license service.
    /// </summary>
    public interface ISimpleLicenseService
    {
        /// <summary>
        /// Validates the license in a very simple manner. This method is wrapper around the <see cref="ILicenseService" />.
        /// </summary>
        /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
        /// <remarks>Note that this method might show a dialog so must be run on the UI thread.</remarks>
        bool Validate();

        /// <summary>
        /// Validates the license on the server. This method is the same as <see cref="SimpleLicenseService.Validate" /> but also checks the server if the license
        /// is valid.
        /// </summary>
        /// <param name="serverUrl">The server URL.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool ValidateOnServer(string serverUrl);
    }
}