// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Client.Example.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using LicenseManager.ViewModels;
    using Models;
    using Services;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private readonly ILicenseService _licenseService;
        private readonly ILicenseValidationService _licenseValidationService;
        private readonly IMessageService _messageService;
        private readonly INetworkLicenseService _networkLicenseService;
        private readonly ILicenseVisualizerService _licenseVisualizerService;
        private readonly IUIVisualizerService _uiVisualizerService;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(ILicenseService licenseService, ILicenseValidationService licenseValidationService,
            IMessageService messageService, INetworkLicenseService networkLicenseService,
            ILicenseVisualizerService licenseVisualizerService, IUIVisualizerService uiVisualizerService)
            : base()
        {
            Argument.IsNotNull(() => licenseService);
            Argument.IsNotNull(() => licenseValidationService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => networkLicenseService);
            Argument.IsNotNull(() => licenseVisualizerService);
            Argument.IsNotNull(() => uiVisualizerService);

            _licenseService = licenseService;
            _licenseValidationService = licenseValidationService;
            _messageService = messageService;
            _networkLicenseService = networkLicenseService;
            _licenseVisualizerService = licenseVisualizerService;
            _uiVisualizerService = uiVisualizerService;

            RemoveLicense = new Command(OnRemoveLicenseExecute);
            ValidateLicenseOnServer = new Command(OnValidateLicenseOnServerExecute, OnValidateLicenseOnServerCanExecute);
            ValidateLicenseOnLocalNetwork = new Command(OnValidateLicenseOnLocalNetworkExecute, OnValidateLicenseOnLocalNetworkCanExecute);
            ShowLicense = new Command(OnShowLicenseExecute);
            ShowLicenseUsage = new Command(OnShowLicenseUsageExecute);

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
            _licenseService.RemoveLicense(LicenseMode.CurrentUser);
            _licenseService.RemoveLicense(LicenseMode.MachineWide);

            await ShowLicenseDialog();
        }

        public Command ValidateLicenseOnServer { get; private set; }

        private bool OnValidateLicenseOnServerCanExecute()
        {
            if (string.IsNullOrWhiteSpace(ServerUri))
            {
                return false;
            }

            if (!_licenseService.AnyLicenseExists())
            {
                return false;
            }

            return true;
        }

        private async void OnValidateLicenseOnServerExecute()
        {
            var licenseString = _licenseService.LoadLicense(LicenseMode.CurrentUser);

            if (string.IsNullOrWhiteSpace(licenseString))
            {
                licenseString = _licenseService.LoadLicense(LicenseMode.MachineWide);
            }

            var result = await _licenseValidationService.ValidateLicenseOnServer(licenseString, ServerUri);

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

        public Command ShowLicense { get; private set; }

        private async void OnShowLicenseExecute()
        {
            await _licenseVisualizerService.ShowLicense();
        }

        public Command ShowLicenseUsage { get; set; }

        private void OnShowLicenseUsageExecute()
        {
            var networkValidationResult = new NetworkValidationResult();

            networkValidationResult.MaximumConcurrentUsers = 2;
            networkValidationResult.CurrentUsers.AddRange(new[]
            {
                new NetworkLicenseUsage("12", "192.168.1.100", "Jon", "Licence signature", DateTime.Now),
                new NetworkLicenseUsage("13", "192.168.1.101", "Jane", "Licence signature", DateTime.Now),
                new NetworkLicenseUsage("14", "192.168.1.102", "Samuel", "Licence signature", DateTime.Now),
                new NetworkLicenseUsage("15", "192.168.1.103", "Paula", "Licence signature", DateTime.Now)
            });

            _uiVisualizerService.ShowDialog<NetworkLicenseUsageViewModel>(networkValidationResult);
        }
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            _networkLicenseService.Validated += OnNetworkLicenseValidated;

            // For debug / demo / test purposes, check every 10 seconds, recommended in production is 30 seconds or higher
            await _networkLicenseService.Initialize(TimeSpan.FromSeconds(10));

            if (_licenseService.AnyLicenseExists())
            {
                var licenseString = _licenseService.LoadExistedLicense();
                var licenseValidation = _licenseValidationService.ValidateLicense(licenseString);

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
            await _licenseVisualizerService.ShowLicense();
        }
        #endregion
    }
}