// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Client.Example.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Services;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private readonly ILicenseService _licenseService;
        private readonly IMessageService _messageService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(ILicenseService licenseService, IMessageService messageService)
            : base()
        {
            Argument.IsNotNull(() => licenseService);
            Argument.IsNotNull(() => messageService);

            _licenseService = licenseService;
            _messageService = messageService;

            RemoveLicense = new Command(OnRemoveLicenseExecute);
            ValidateLicenseOnServer = new Command(OnValidateLicenseOnServerExecute, OnValidateLicenseOnServerCanExecute);

            ServerUri = string.Format("http://localhost:1815/api/license/validate");
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

        public string ServerUri { get; set; }
        #endregion

        #region Commands
        public Command RemoveLicense { get; private set; }

        private async void OnRemoveLicenseExecute()
        {
            _licenseService.RemoveLicense();

            await ShowLicenseDialog();
        }

        public Command ValidateLicenseOnServer { get; private set; }

        private bool OnValidateLicenseOnServerCanExecute()
        {
            if (string.IsNullOrWhiteSpace(ServerUri))
            {
                return false;
            }

            if (!_licenseService.LicenseExists())
            {
                return false;
            }

            return true;
        }

        private async void OnValidateLicenseOnServerExecute()
        {
            var licenseString = _licenseService.LoadLicense();

            var result = await _licenseService.ValidateLicenseOnServer(licenseString, ServerUri);

            await _messageService.Show(string.Format("License is {0}valid", result ? string.Empty : "NOT "));
        }
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            if (_licenseService.LicenseExists())
            {
                var licenseString = _licenseService.LoadLicense();
                var licenseValidation = _licenseService.ValidateLicense(licenseString);

                if (licenseValidation.HasErrors)
                {
                    await ShowLicenseDialog();
                }
            }
            else
            {
                await ShowLicenseDialog();
            }
        }

        private async Task ShowLicenseDialog()
        {
            await _licenseService.ShowSingleLicenseDialog("Catel", "/Orc.LicenseManager.Client.Example;component/Resources/Images/logo_with_text.png", "Catel is a company made in 2010 and is  dolor sit amet, consectetur adipiscing elit. Etiam nec sem sit amet felis blandit semper. Morbi tempus ligula urna, feugiat rhoncus dolor elementum non.", "http://www.catelproject.com/", "CatelSoftware License Required", "http://www.catelproject.com/product/buy/642");
            //_licenseGenerationService.ShowSingleLicenseDialog("Orchestra", "http://staugustineorchestra.org/wp-content/uploads/2012/08/Violin-Logos-Color-Fin.jpg", "Orchestra is a project that has 2 sides a server and a shell sit amet, consectetur adipiscing elit. Etiam nec sem sit amet felis blandit semper. Morbi tempus ligula urna, feugiat rhoncus dolor elementum non.", "http://www.orchestra.com/", "Orchestra License Required", "http://www.orchestra.com/product/buy/642");
            //_licenseGenerationService.ShowSingleLicenseDialog();
        }
        #endregion
    }
}