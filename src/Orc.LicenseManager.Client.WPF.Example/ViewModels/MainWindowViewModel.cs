// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Client.Example.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Models;
    using Services;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private readonly ILicenseService _licenseService;
        private readonly IMessageService _messageService;
        private readonly INetworkLicenseService _networkLicenseService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(ILicenseService licenseService, IMessageService messageService, INetworkLicenseService networkLicenseService)
            : base()
        {
            Argument.IsNotNull(() => licenseService);
            Argument.IsNotNull(() => messageService);

            _licenseService = licenseService;
            _messageService = messageService;
            _networkLicenseService = networkLicenseService;

            RemoveLicense = new Command(OnRemoveLicenseExecute);
            ValidateLicenseOnServer = new Command(OnValidateLicenseOnServerExecute, OnValidateLicenseOnServerCanExecute);
            ValidateLicenseOnLocalNetwork = new Command(OnValidateLicenseOnLocalNetworkExecute, OnValidateLicenseOnLocalNetworkCanExecute);

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

            await _messageService.Show(string.Format("License is {0}valid", result.IsValid ? string.Empty : "NOT "));
        }

        public Command ValidateLicenseOnLocalNetwork { get; private set; }

        private bool OnValidateLicenseOnLocalNetworkCanExecute()
        {
            var license = _licenseService.CurrentLicense;
            return (license != null);
        }

        private async void OnValidateLicenseOnLocalNetworkExecute()
        {
            NetworkValidationResult validationResult = null;

            validationResult = await _networkLicenseService.ValidateLicense();

            await _messageService.Show(string.Format("License is {0}valid, using '{1}' of '{2}' licenses", validationResult.IsValid ? string.Empty : "NOT ", validationResult.CurrentUsers.Count, validationResult.MaximumConcurrentUsers));
        }
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            _networkLicenseService.Validated += OnNetworkLicenseValidated;

            // For debug / demo / test purposes, check every 10 seconds, recommended in production is 30 seconds or higher
            await _networkLicenseService.Initialize(TimeSpan.FromSeconds(10));

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

        private async void OnNetworkLicenseValidated(object sender, NetworkValidatedEventArgs e)
        {
            var validationResult = e.ValidationResult;
            if (!validationResult.IsValid)
            {
                var latestUsage = validationResult.GetLatestUser();

                if (validationResult.IsCurrentUserLatestUser())
                {
                    await _messageService.Show(string.Format("License is invalid, using '{0}' of '{1}' licenses. You are the latest user, your software will be shut down", validationResult.CurrentUsers.Count, validationResult.MaximumConcurrentUsers));                    
                }
                else
                {
                    await _messageService.Show(string.Format("License is invalid, using '{0}' of '{1}' licenses. The latest user is '{2}' with ip '{3}', you can continue working", validationResult.CurrentUsers.Count, validationResult.MaximumConcurrentUsers, latestUsage.UserName, latestUsage.Ip));
                }
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