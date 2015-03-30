// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleLicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Threading.Tasks;
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
        private readonly ILicenseVisualizerService _licenseVisualizerService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleLicenseService" /> class.
        /// </summary>
        /// <param name="licenseService">The license service.</param>
        /// <param name="licenseVisualizerService">The license visualizer service.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="licenseService" /> is <c>null</c>.</exception>
        public SimpleLicenseService(ILicenseService licenseService, ILicenseVisualizerService licenseVisualizerService)
        {
            Argument.IsNotNull(() => licenseService);
            Argument.IsNotNull(() => licenseVisualizerService);

            _licenseService = licenseService;
            _licenseVisualizerService = licenseVisualizerService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validates the license on the server. This method is the same as <see cref="Validate"/> but also checks the server if the license
        /// is valid.
        /// </summary>
        /// <param name="serverUrl">The server URL.</param>
        /// <param name="applicationId">The application identifier.</param>
        /// <param name="aboutTitle">The about title.</param>
        /// <param name="aboutImage">The about image.</param>
        /// <param name="aboutText">The about text.</param>
        /// <param name="aboutSiteUrl">The about site URL.</param>
        /// <param name="title">The title.</param>
        /// <param name="purchaseLinkUrl">The purchase link URL.</param>
        /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
        public async Task<bool> ValidateOnServer(string serverUrl, string applicationId, string aboutTitle, string aboutImage, string aboutText, string aboutSiteUrl = null, string title = null, string purchaseLinkUrl = null)
        {
            await _licenseService.Initialize(applicationId);

            if (!_licenseService.LicenseExists())
            {
                await _licenseVisualizerService.ShowLicense(aboutTitle, aboutImage, aboutText, aboutSiteUrl, title, purchaseLinkUrl);
            }

            if (!_licenseService.LicenseExists())
            {
                return false;
            }

            var licenseString = _licenseService.LoadLicense();

            // Server first so it's possible to make licenses invalid
            var licenseValidationResult = await _licenseService.ValidateLicenseOnServer(licenseString, serverUrl);
            if (!licenseValidationResult.IsValid)
            {
                Log.Error("The server returned that the license is invalid and contains the following errors:");
                Log.Error("  * {0}", licenseValidationResult.AdditionalInfo);

                return false;
            }

            Log.Debug("Server returned valid license, doing a local check to be sure that the server wasn't forged");

            var validationContext = _licenseService.ValidateLicense(licenseString);
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
        /// <param name="applicationId">The application identifier, can be any value but should be unique.</param>
        /// <param name="aboutTitle">The about title.</param>
        /// <param name="aboutImage">The about image.</param>
        /// <param name="aboutText">The about text.</param>
        /// <param name="aboutSiteUrl">The about site.</param>
        /// <param name="title">The title.</param>
        /// <param name="purchaseLinkUrl">The purchase link.</param>
        /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
        /// <remarks>Note that this method might show a dialog so must be run on the UI thread.</remarks>
        public async Task<bool> Validate(string applicationId, string aboutTitle, string aboutImage, string aboutText, string aboutSiteUrl = null, string title = null, string purchaseLinkUrl = null)
        {
            await _licenseService.Initialize(applicationId);

            if (!_licenseService.LicenseExists())
            {
                await _licenseVisualizerService.ShowLicense(aboutTitle, aboutImage, aboutText, aboutSiteUrl, title, purchaseLinkUrl);
            }

            if (!_licenseService.LicenseExists())
            {
                return false;
            }

            var licenseString = _licenseService.LoadLicense();
            var licenseValidation = _licenseService.ValidateLicense(licenseString);

            return !licenseValidation.HasErrors;
        }
        #endregion
    }
}