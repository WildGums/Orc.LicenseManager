// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DialogRequestLicenseService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using Catel;
    using Catel.Logging;
    using Catel.Services;
    using ViewModels;

    public class DialogLicenseVisualizerService : ILicenseVisualizerService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly ILicenseInfoService _licenseInfoService;
        private readonly IDispatcherService _dispatcherService;

        #region Constructors
        public DialogLicenseVisualizerService(IUIVisualizerService uiVisualizerService, ILicenseInfoService licenseInfoService,
            IDispatcherService dispatcherService)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => licenseInfoService);
            Argument.IsNotNull(() => dispatcherService);

            _uiVisualizerService = uiVisualizerService;
            _licenseInfoService = licenseInfoService;
            _dispatcherService = dispatcherService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Shows the single license dialog including all company info. You will see the about box.
        /// </summary>
        public void ShowLicense()
        {
            Log.Debug("Showing license dialog with company info");

#pragma warning disable AvoidAsyncVoid
            _dispatcherService.Invoke(async () =>
            {
                var licenseInfo = _licenseInfoService.GetLicenseInfo();
                await _uiVisualizerService.ShowDialogAsync<LicenseViewModel>(licenseInfo);
            }, true);
#pragma warning restore AvoidAsyncVoid
        }
        #endregion
    }
}