namespace Orc.LicenseManager
{
    using System.Threading.Tasks;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using ViewModels;

    public static class WpfNetworkValidationHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static bool _isInErrorHandling;

        public static async Task DefaultNetworkLicenseServiceValidationHandlerAsync(object sender, NetworkValidatedEventArgs e)
        {
            if (_isInErrorHandling)
            {
                Log.Warning("Already handling the invalid license usage");
                return;
            }

            var validationResult = e.ValidationResult;
            if (!validationResult.IsValid && validationResult.IsCurrentUserLatestUser())
            {
                _isInErrorHandling = true;

                var serviceLocator = ServiceLocator.Default;

                var dispatcherService = serviceLocator.ResolveRequiredType<IDispatcherService>();
                await dispatcherService.InvokeTaskAsync(async () =>
                {
                    var uiVisualizerService = serviceLocator.ResolveRequiredType<IUIVisualizerService>();
                    await uiVisualizerService.ShowDialogAsync<NetworkLicenseUsageViewModel>(validationResult);
                });

                _isInErrorHandling = false;

                // Force check
                var networkLicenseService = serviceLocator.ResolveRequiredType<INetworkLicenseService>();
                await networkLicenseService.ValidateLicenseAsync();
            }
        }
    }
}
