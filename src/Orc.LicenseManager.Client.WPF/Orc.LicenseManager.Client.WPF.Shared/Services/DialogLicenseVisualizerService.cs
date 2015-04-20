// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DialogRequestLicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.Services;
    using ViewModels;

    public class DialogLicenseVisualizerService : ILicenseVisualizerService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly ILicenseInfoService _licenseInfoService;

        #region Constructors
        public DialogLicenseVisualizerService(IUIVisualizerService uiVisualizerService, ILicenseInfoService licenseInfoService)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => licenseInfoService);

            _uiVisualizerService = uiVisualizerService;
            _licenseInfoService = licenseInfoService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Shows the single license dialog including all company info. You will see the about box.
        /// </summary>
        public async Task ShowLicense()
        {
            Log.Debug("Showing license dialog with company info");

            var licenseInfo = _licenseInfoService.GetLicenseInfo();
            _uiVisualizerService.ShowDialog<LicenseViewModel>(licenseInfo);
        }
        #endregion
    }
}