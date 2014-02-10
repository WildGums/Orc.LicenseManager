// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Reflection;
    using Catel;
    using Catel.Fody;
    using Catel.MVVM.Services;
    using Catel.Reflection;
    using ViewModels;

    /// <summary>
    /// Service to validate, store and remove licenses for software products.
    /// </summary>
    public class LicenseService : ILicenseService
    {
        private readonly IUIVisualizerService _uiVisualizerService;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseService" /> class.
        /// </summary>
        /// <param name="uiVisualizerService">The UI visualizer service.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="uiVisualizerService" /> is <c>null</c>.</exception>
        public LicenseService(IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => uiVisualizerService);

            _uiVisualizerService = uiVisualizerService;
        }
        #endregion

        #region ILicenseService Members
        /// <summary>
        /// Initializes the specified application identifier.
        /// </summary>
        /// <param name="applicationId">The application identifier.</param>
        /// <exception cref="ArgumentException">The <paramref name="applicationId"/> is <c>null</c> or whitespace.</exception>
        public void Initialize([NotNullOrWhitespace] string applicationId)
        {
        }

        /// <summary>
        /// Shows the single license dialog.
        /// </summary>
        /// <param name="title">The title. If <c>null</c>, the title will be extracted from the entry assembly.</param>
        /// <param name="website">The website.  If <c>null</c>, no website link will be displayed.</param>
        public void ShowSingleLicenseDialog(string title = null, string website = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                var assembly = Assembly.GetExecutingAssembly() ?? Assembly.GetEntryAssembly();
                title = assembly.Title();
            }

            var vm = new SingleLicenseViewModel();
            _uiVisualizerService.ShowDialog(vm);
        }
        #endregion
    }
}