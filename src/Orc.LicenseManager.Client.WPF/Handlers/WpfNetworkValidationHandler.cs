namespace Orc.LicenseManager;

using System.Threading.Tasks;
using Catel.IoC;
using Catel.Logging;
using Catel.Services;
using Microsoft.Extensions.Logging;
using ViewModels;

public class WpfNetworkValidationHandler : INetworkValidationHandler
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(WpfNetworkValidationHandler));

    private readonly INetworkLicenseService _networkLicenseService;
    private readonly IUIVisualizerService _uiVisualizerService;

    private bool _isInErrorHandling;

    public WpfNetworkValidationHandler(INetworkLicenseService networkLicenseService,
        IUIVisualizerService uiVisualizerService)
    {
        _networkLicenseService = networkLicenseService;
        _uiVisualizerService = uiVisualizerService;
    }

    public async void HandleNetworkValidation(object? sender, NetworkValidatedEventArgs e)
    {
        if (_isInErrorHandling)
        {
            Logger.LogWarning("Already handling the invalid license usage");
            return;
        }

        var validationResult = e.ValidationResult;
        if (!validationResult.IsValid && 
            _networkLicenseService.IsCurrentUserLatestUser(validationResult))
        {
            _isInErrorHandling = true;

            await _uiVisualizerService.ShowDialogAsync<NetworkLicenseUsageViewModel>(validationResult);

            _isInErrorHandling = false;

            // Force check
            _ = _networkLicenseService.ValidateLicenseAsync();
        }
    }
}
