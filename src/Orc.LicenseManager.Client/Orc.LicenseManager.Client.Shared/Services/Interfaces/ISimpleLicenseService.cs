// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISimpleLicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System.Threading.Tasks;

    /// <summary>
    /// A very simple implementation of the license service.
    /// </summary>
    public interface ISimpleLicenseService
    {
        /// <summary>
        /// Validates the license in a very simple manner. This method is wrapper around the <see cref="ILicenseService" />.
        /// </summary>
        /// <param name="applicationId">The application identifier, can be any value but should be unique.</param>
        /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
        /// <remarks>Note that this method might show a dialog so must be run on the UI thread.</remarks>
        Task<bool> Validate(string applicationId);

        /// <summary>
        /// Validates the license on the server. This method is the same as <see cref="SimpleLicenseService.Validate" /> but also checks the server if the license
        /// is valid.
        /// </summary>
        /// <param name="serverUrl">The server URL.</param>
        /// <param name="applicationId">The application identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        Task<bool> ValidateOnServer(string serverUrl, string applicationId);
    }
}