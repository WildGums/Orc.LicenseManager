namespace Orc.LicenseManager;

using System;
using Catel.Logging;
using Catel.Services;
using Microsoft.Extensions.Logging;
using ViewModels;

public class DialogLicenseVisualizerService : ILicenseVisualizerService
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(DialogLicenseVisualizerService));

    private readonly IUIVisualizerService _uiVisualizerService;
    private readonly ILicenseInfoService _licenseInfoService;
    private readonly IDispatcherService _dispatcherService;

    public DialogLicenseVisualizerService(IUIVisualizerService uiVisualizerService, ILicenseInfoService licenseInfoService,
        IDispatcherService dispatcherService)
    {
        _uiVisualizerService = uiVisualizerService;
        _licenseInfoService = licenseInfoService;
        _dispatcherService = dispatcherService;
    }

    /// <summary>
    /// Shows the single license dialog including all company info. You will see the about box.
    /// </summary>
    public void ShowLicense()
    {
        Logger.LogDebug("Showing license dialog with company info");

#pragma warning disable AvoidAsyncVoid
        _dispatcherService.Invoke(async () =>
        {
            var licenseInfo = _licenseInfoService.GetLicenseInfo();
            await _uiVisualizerService.ShowDialogAsync<LicenseViewModel>(licenseInfo);
        }, true);
#pragma warning restore AvoidAsyncVoid
    }
}
