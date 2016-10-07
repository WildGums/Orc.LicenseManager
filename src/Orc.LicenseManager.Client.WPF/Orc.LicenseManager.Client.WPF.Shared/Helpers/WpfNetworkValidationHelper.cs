// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WpfNetworkValidationHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System.Threading.Tasks;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Services;
    using ViewModels;

    public static class WpfNetworkValidationHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static bool _isInErrorHandling;

        [ObsoleteEx(ReplacementTypeOrMember = "DefaultNetworkLicenseServiceValidationHandlerAsync", TreatAsErrorFromVersion = "1.1.0", RemoveInVersion = "2.0.0")]
        public static void DefaultNetworkLicenseServiceValidationHandler(object sender, NetworkValidatedEventArgs e)
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
                    networkLicenseService.ValidateLicense();
                }
            }
        }

        public static async Task DefaultNetworkLicenseServiceValidationHandlerAsync(object sender, NetworkValidatedEventArgs e)
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

                    var dispatcherService = serviceLocator.ResolveType<IDispatcherService>();
                    await dispatcherService.InvokeAsync(async () =>
                    {
                        var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
                        await uiVisualizerService.ShowDialogAsync<NetworkLicenseUsageViewModel>(validationResult);
                    });

                    _isInErrorHandling = false;

                    // Force check
                    var networkLicenseService = serviceLocator.ResolveType<INetworkLicenseService>();
                    networkLicenseService.ValidateLicense();
                }
            }
        }
    }
}