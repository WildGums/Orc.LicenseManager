// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WpfNetworkValidationHelper.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Services;
    using ViewModels;

    public static class WpfNetworkValidationHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static bool _isInErrorHandling;

        public static async void DefaultNetworkLicenseServiceValidationHandler(object sender, NetworkValidatedEventArgs e)
        {
            if (_isInErrorHandling)
            {
                Log.Warning("Already handling the invalid license usage");
                return;
            }

            var validationResult = e.ValidationResult;
            if (!validationResult.IsValid)
            {
                if (validationResult.IsCurrentUserLatestUser())
                {
                    _isInErrorHandling = true;

                    var serviceLocator = ServiceLocator.Default;

                    var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
                    uiVisualizerService.ShowDialog<NetworkLicenseUsageViewModel>(validationResult);

                    _isInErrorHandling = false;

                    // Force check
                    var networkLicenseService = serviceLocator.ResolveType<INetworkLicenseService>();
                    await networkLicenseService.ValidateLicense();
                }
            }
        }
    }
}