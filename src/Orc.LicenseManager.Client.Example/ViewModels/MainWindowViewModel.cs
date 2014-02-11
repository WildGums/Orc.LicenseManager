// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Client.Example.ViewModels
{
    using Catel.MVVM;
    using Services;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Commands

        #region Properties
        public Command ShowLicense { get; private set; }
        #endregion

        #region Methods
        private void OnShowLicenseExecute()
        {
            _licenseService.ShowSingleLicenseDialog("CatelSoftware", "http://www.catelproject.com");
        }
        #endregion

        #endregion

        #region Fields
        /// <summary>
        /// Register the License property so it is known in the class.
        /// </summary>
        #endregion

        #region Fields
        private readonly ILicenseService _licenseService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(ILicenseService licenseService)
            : base()
        {
            _licenseService = licenseService;

            ShowLicense = new Command(OnShowLicenseExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return "License manager example"; }
        }
        #endregion
    }
}