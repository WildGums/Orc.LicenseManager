// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INetworkLicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using Models;

    public interface INetworkLicenseService
    {
        #region Methods
        /// <summary>
        /// Initializes the service.
        /// </summary>
        /// <param name="pollingInterval">The polling interval. If <c>default(TimeSpan)</c>, no polling will be enabled.</param>
        /// <returns>Task.</returns>
        /// <remarks>Note that this method is optional but will start the service. If this method is not called, the service will be initialized
        /// in the <see cref="ValidateLicense" /> method.</remarks>
        void Initialize(TimeSpan pollingInterval = default(TimeSpan));

        NetworkValidationResult ValidateLicense();
        #endregion

        /// <summary>
        /// Gets or sets the search timeout for other licenses on the network.
        /// </summary>
        /// <value>The search timeout.</value>
        TimeSpan SearchTimeout { get; set; }

        /// <summary>
        /// Gets the polling interval on how often to check the network.
        /// </summary>
        /// <value>The search timeout.</value>
        TimeSpan PollingInterval { get; }

        /// <summary>
        /// Gets the computer identifier.
        /// </summary>
        /// <value>The computer identifier.</value>
        string ComputerId { get; }

        /// <summary>
        /// Occurs every time when the network validation has finished.
        /// </summary>
        event EventHandler<NetworkValidatedEventArgs> Validated;
    }
}