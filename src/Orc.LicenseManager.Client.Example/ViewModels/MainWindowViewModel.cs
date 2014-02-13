// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Client.Example.ViewModels
{
    using Catel;
    using Catel.MVVM;
    using Services;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
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
            Argument.IsNotNull(() => licenseService);
            RemoveLicense = new Command(OnRemoveLicenseExecute);

            _licenseService = licenseService;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the RemoveLicense command.
        /// </summary>
        public Command RemoveLicense { get; private set; }

        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return "License manager example"; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the RemoveLicense command is executed.
        /// </summary>
        private void OnRemoveLicenseExecute()
        {
            _licenseService.RemoveLicense();
            ShowLicenseDialogue();
        }

        protected override void Initialize()
        {
            if (_licenseService.LicenseExists())
            {
                var licenseString = _licenseService.LoadLicense();
                var licenseValidation = _licenseService.ValidateLicense(licenseString);

                if (licenseValidation.HasErrors)
                {
                    ShowLicenseDialogue();
                }
            }
            else
            {
                ShowLicenseDialogue();
            }
        }

        private void ShowLicenseDialogue()
        {
            _licenseService.ShowSingleLicenseDialog("Catel", "http://www.catelproject.com/wp-content/uploads/2013/10/logo_with_text.png", 
                "Catel is a company made in 2010 and is  dolor sit amet, consectetur adipiscing elit. Etiam nec sem sit amet felis blandit semper. Morbi tempus ligula urna, feugiat rhoncus dolor elementum non.", "http://www.catelproject.com/", "CatelSoftware License Required", "http://www.catelproject.com/product/buy/642");
            //_licenseService.ShowSingleLicenseDialog();
        }
        #endregion
    }
}