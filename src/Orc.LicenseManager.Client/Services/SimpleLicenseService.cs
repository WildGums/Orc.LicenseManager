// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleLicenseService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;
    using Catel;
    using Catel.Logging;

    /// <summary>
    /// Simple license service.
    /// </summary>
    public class SimpleLicenseService : ISimpleLicenseService
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ILicenseService _licenseService;
        private readonly ILicenseValidationService _licenseValidationService;
        private readonly ILicenseVisualizerService _licenseVisualizerService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleLicenseService" /> class.
        /// </summary>
        /// <param name="licenseService">The license service.</param>
        /// <param name="licenseValidationService">The license validation service.</param>
        /// <param name="licenseVisualizerService">The license visualizer service.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="licenseService" /> is <c>null</c>.</exception>
        public SimpleLicenseService(ILicenseService licenseService, ILicenseValidationService licenseValidationService, 
            ILicenseVisualizerService licenseVisualizerService)
        {
            Argument.IsNotNull(() => licenseService);
            Argument.IsNotNull(() => licenseValidationService);
            Argument.IsNotNull(() => licenseVisualizerService);

            _licenseService = licenseService;
            _licenseValidationService = licenseValidationService;
            _licenseVisualizerService = licenseVisualizerService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validates the license on the server. This method is the same as <see cref="Validate" /> but also checks the server if the license
        /// is valid.
        /// </summary>
        /// <param name="serverUrl">The server URL.</param>
        /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
        public bool ValidateOnServer(string serverUrl)
        {
            if (!EnsureLicenseExists())
            {
                return false;
            }

            var licenseString = _licenseService.LoadExistingLicense();
            if (string.IsNullOrWhiteSpace(licenseString))
            {
                return false;
            }

            // Server first so it's possible to make licenses invalid
            var licenseValidationResult = _licenseValidationService.ValidateLicenseOnServer(licenseString, serverUrl);
            if (!licenseValidationResult.IsValid)
            {
                Log.Error("The server returned that the license is invalid and contains the following errors:");
                Log.Error("  * {0}", licenseValidationResult.AdditionalInfo);

                return false;
            }

            Log.Debug("Server returned valid license, doing a local check to be sure that the server wasn't forged");

            var validationContext = _licenseValidationService.ValidateLicense(licenseString);
            if (validationContext.HasErrors)
            {
                Log.Error("The license is invalid and contains the following errors:");
                foreach (var error in validationContext.GetErrors())
                {
                    Log.Error("  * {0}", error.Message);
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the license in a very simple manner. This method is wrapper around the <see cref="ILicenseService" />.
        /// </summary>
        /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
        /// <remarks>Note that this method might show a dialog so must be run on the UI thread.</remarks>
        public bool Validate()
        {
            if (!EnsureLicenseExists())
            {
                return false;
            }

            var licenseString = _licenseService.LoadExistingLicense();
            if (string.IsNullOrWhiteSpace(licenseString))
            {
                return false;
            }

            var licenseValidation = _licenseValidationService.ValidateLicense(licenseString);

            return !licenseValidation.HasErrors;
        }

        private bool EnsureLicenseExists()
        {
            if (!_licenseService.AnyExistingLicense())
            {
                _licenseVisualizerService.ShowLicense();
            }

            return _licenseService.AnyExistingLicense();
        }
        #endregion
    }
}
