// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkLicenseUsageViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Catel.Threading;
    using Models;
    using Services;

    public class NetworkLicenseUsageViewModel : ViewModelBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ILicenseInfoService _licenseInfoService;
        private readonly IProcessService _processService;
        private readonly INetworkLicenseService _networkLicenseService;
        private readonly IDispatcherService _dispatcherService;

        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        #endregion

        #region Constructors
        public NetworkLicenseUsageViewModel(NetworkValidationResult networkValidationResult, ILicenseInfoService licenseInfoService, 
            IProcessService processService, INetworkLicenseService networkLicenseService, IDispatcherService dispatcherService)
        {
            Argument.IsNotNull(() => networkValidationResult);
            Argument.IsNotNull(() => licenseInfoService);
            Argument.IsNotNull(() => processService);
            Argument.IsNotNull(() => networkLicenseService);
            Argument.IsNotNull(() => dispatcherService);

            _licenseInfoService = licenseInfoService;
            _processService = processService;
            _networkLicenseService = networkLicenseService;
            _dispatcherService = dispatcherService;

            var assembly = AssemblyHelper.GetEntryAssembly();
            Title = assembly.Title() + " licence usage";
            PurchaseUrl = _licenseInfoService.GetLicenseInfo().PurchaseUrl;
            UpdateValidationResult(networkValidationResult, false);

            _dispatcherTimer.Interval = TimeSpan.FromSeconds(15);

            CloseApplication = new Command(OnCloseApplicationExecute);
            BuyLicenses = new Command(OnBuyLicensesExecute);
        }
        #endregion

        #region Properties
        public string PurchaseUrl { get; set; }

        public List<NetworkLicenseUsage> CurrentUsers { get; set; }

        public int MaximumNumberOfConcurrentUsages { get; set; }
        #endregion

        #region Commands
        public Command CloseApplication { get; private set; }

        private void OnCloseApplicationExecute()
        {
            Log.Info("Closing application");

            var process = Process.GetCurrentProcess();
            process.Kill();
        }

        public Command BuyLicenses { get; private set; }

        private void OnBuyLicensesExecute()
        {
            var purchaseUrl = PurchaseUrl;

            Log.Info("Buying licenses using url '{0}'", purchaseUrl);

            _processService.StartProcess(purchaseUrl);
        }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _dispatcherTimer.Tick += OnDispatcherTimerTick;
            _networkLicenseService.Validated += OnNetworkLicenseValidated;

            _dispatcherTimer.Start();
        }

        protected override async Task CloseAsync()
        {
            _dispatcherTimer.Stop();

            _dispatcherTimer.Tick -= OnDispatcherTimerTick;
            _networkLicenseService.Validated -= OnNetworkLicenseValidated;

            await base.CloseAsync();
        }

#pragma warning disable AvoidAsyncVoid
        private async void OnDispatcherTimerTick(object sender, EventArgs e)
#pragma warning restore AvoidAsyncVoid
        {
            var validationResult = await TaskHelper.Run(() => _networkLicenseService.ValidateLicense(), true);

            UpdateValidationResult(validationResult);
        }

        private void OnNetworkLicenseValidated(object sender, NetworkValidatedEventArgs e)
        {
            UpdateValidationResult(e.ValidationResult);
        }

        private void UpdateValidationResult(NetworkValidationResult networkValidationResult, bool allowToClose = true)
        {
            Argument.IsNotNull(() => networkValidationResult);

            var computerId = _networkLicenseService.ComputerId;

            MaximumNumberOfConcurrentUsages = networkValidationResult.MaximumConcurrentUsers;
            CurrentUsers = (from user in networkValidationResult.CurrentUsers
                            where !string.Equals(user.ComputerId, computerId)
                            select user).ToList();

            if (allowToClose && networkValidationResult.IsValid)
            {
                Log.Info("No longer exceeding maximum concurrent users, closing network license validation");

#pragma warning disable 4014
                _dispatcherService.BeginInvoke(() => this.SaveAndCloseViewModelAsync());
#pragma warning restore 4014
            }
        }
        #endregion
    }
}