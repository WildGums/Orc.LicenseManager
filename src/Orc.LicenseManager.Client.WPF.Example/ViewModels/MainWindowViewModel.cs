// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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
            ValidateLicenseOnServer = new TaskCommand(OnValidateLicenseOnServerExecuteAsync, OnValidateLicenseOnServerCanExecute);
            ValidateLicenseOnLocalNetwork = new TaskCommand(OnValidateLicenseOnLocalNetworkExecuteAsync, OnValidateLicenseOnLocalNetworkCanExecute);
            ShowLicense = new Command(OnShowLicenseExecute);
            ShowLicenseUsage = new TaskCommand(OnShowLicenseUsageExecuteAsync);

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
            get { return "Orc.LicenseManager example"; }
        }

        public string ServerUri { get; set; }
        #endregion

        #region Commands
        public Command RemoveLicense { get; private set; }

        private void OnRemoveLicenseExecute()
        {
            _licenseService.RemoveLicense(LicenseMode.CurrentUser);
            _licenseService.RemoveLicense(LicenseMode.MachineWide);

            ShowLicenseDialog();
        }

        public TaskCommand ValidateLicenseOnServer { get; private set; }

        private bool OnValidateLicenseOnServerCanExecute()
        {
            if (string.IsNullOrWhiteSpace(ServerUri))
            {
                return false;
            }

            if (!_licenseService.AnyExistingLicense())
            {
                return false;
            }

            return true;
        }

        private async Task OnValidateLicenseOnServerExecuteAsync()
        {
            var licenseString = _licenseService.LoadLicense(LicenseMode.CurrentUser);

            if (string.IsNullOrWhiteSpace(licenseString))
            {
                licenseString = _licenseService.LoadLicense(LicenseMode.MachineWide);
            }

            var result = await _licenseValidationService.ValidateLicenseOnServerAsync(licenseString, ServerUri);

            await _messageService.ShowAsync(string.Format("License is {0}valid", result.IsValid ? string.Empty : "NOT "));
        }

        public TaskCommand ValidateLicenseOnLocalNetwork { get; private set; }

        private bool OnValidateLicenseOnLocalNetworkCanExecute()
        {
            if (!_licenseService.AnyExistingLicense())
            {
                return false;
            }

            return true;
        }

        private async Task OnValidateLicenseOnLocalNetworkExecuteAsync()
        {
            NetworkValidationResult validationResult = null;

            validationResult = await _networkLicenseService.ValidateLicenseAsync();

            await _messageService.ShowAsync(string.Format("License is {0}valid, using '{1}' of '{2}' licenses", validationResult.IsValid ? string.Empty : "NOT ", validationResult.CurrentUsers.Count, validationResult.MaximumConcurrentUsers));
        }

        public Command ShowLicense { get; private set; }

        private void OnShowLicenseExecute()
        {
            _licenseVisualizerService.ShowLicense();
        }

        public TaskCommand ShowLicenseUsage { get; set; }

        private async Task OnShowLicenseUsageExecuteAsync()
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

            await _uiVisualizerService.ShowDialogAsync<NetworkLicenseUsageViewModel>(networkValidationResult);
        }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            _networkLicenseService.Validated += OnNetworkLicenseValidated;

            // For debug / demo / test purposes, check every 10 seconds, recommended in production is 30 seconds or higher
            await Task.Factory.StartNew(() => _networkLicenseService.Initialize(TimeSpan.FromSeconds(10)));

            if (_licenseService.AnyExistingLicense())
            {
                var licenseString = _licenseService.LoadExistingLicense();
                var licenseValidation = await _licenseValidationService.ValidateLicenseAsync(licenseString);

                if (licenseValidation.HasErrors)
                {
                    ShowLicenseDialog();
                }
            }
            else
            {
                ShowLicenseDialog();
            }
        }

#pragma warning disable AvoidAsyncVoid
        private async void OnNetworkLicenseValidated(object sender, NetworkValidatedEventArgs e)
#pragma warning restore AvoidAsyncVoid
        {
            var validationResult = e.ValidationResult;
            if (!validationResult.IsValid)
            {
                var latestUsage = validationResult.GetLatestUser();

                if (validationResult.IsCurrentUserLatestUser())
                {
                    await _messageService.ShowAsync(string.Format("License is invalid, using '{0}' of '{1}' licenses. You are the latest user, your software will be shut down", validationResult.CurrentUsers.Count, validationResult.MaximumConcurrentUsers));
                }
                else
                {
                    await _messageService.ShowAsync(string.Format("License is invalid, using '{0}' of '{1}' licenses. The latest user is '{2}' with ip '{3}', you can continue working", validationResult.CurrentUsers.Count, validationResult.MaximumConcurrentUsers, latestUsage.UserName, latestUsage.Ip));
                }
            }
        }

        private void ShowLicenseDialog()
        {
            _licenseVisualizerService.ShowLicense();
        }
        #endregion
    }
}
