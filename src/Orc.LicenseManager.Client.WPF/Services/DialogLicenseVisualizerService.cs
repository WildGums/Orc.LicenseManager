// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DialogRequestLicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Models;
    using ViewModels;

    public class DialogLicenseVisualizerService : ILicenseVisualizerService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IViewModelFactory _viewModelFactory;

        #region Constructors
        public DialogLicenseVisualizerService(IUIVisualizerService uiVisualizerService, IViewModelFactory viewModelFactory)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => viewModelFactory);

            _uiVisualizerService = uiVisualizerService;
            _viewModelFactory = viewModelFactory;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Shows the single license dialog including all company info. You will see the about box.
        /// </summary>
        /// <param name="aboutTitle">The title inside the about box.</param>
        /// <param name="aboutImage">The about box image.</param>
        /// <param name="aboutText">The text inside the about box</param>
        /// <param name="aboutSiteUrl">The site inside the about box.</param>
        /// <param name="title">The title. If <c>null</c>, the title will be extracted from the entry assembly.</param>
        /// <param name="purchaseLinkUrl">The url to the store. If <c>null</c>, no purchaseLinkUrl link will be displayed.</param>
        public async Task ShowLicense(string aboutTitle, string aboutImage, string aboutText, string aboutSiteUrl = null, string title = null, string purchaseLinkUrl = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                var assembly = AssemblyHelper.GetEntryAssembly();
                title = assembly.Title();
            }

            var model = new SingleLicenseModel
            {
                Title = title,
                PurchaseLink = purchaseLinkUrl,
                AboutImage = aboutImage,
                AboutTitle = aboutTitle,
                AboutText = aboutText,
                AboutSite = aboutSiteUrl
            };

            Log.Info("Showing license dialog with company info");

            var vm = _viewModelFactory.CreateViewModel<SingleLicenseViewModel>(model);
            await _uiVisualizerService.ShowDialog(vm);
        }

        /// <summary>
        /// Shows the single license dialog. You won't see the about box.
        /// </summary>
        /// <param name="title">The title. If <c>null</c>, the title will be extracted from the entry assembly.</param>
        /// <param name="purchaseLink">The url to the store. If <c>null</c>, no purchaseLinkUrl link will be displayed.</param>
        public async Task ShowLicense(string title = null, string purchaseLink = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                var assembly = AssemblyHelper.GetEntryAssembly();
                title = assembly.Title();
            }

            var model = new SingleLicenseModel
            {
                Title = title,
                PurchaseLink = purchaseLink
            };

            Log.Info("Showing license dialog");

            var vm = _viewModelFactory.CreateViewModel<SingleLicenseViewModel>(model);
            await _uiVisualizerService.ShowDialog(vm);
        }
        #endregion
    }
}