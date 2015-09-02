// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkLicenseUsageViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Models;
    using Services;

    public class NetworkLicenseUsageViewModel : ViewModelBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly NetworkValidationResult _networkValidationResult;
        private readonly ILicenseInfoService _licenseInfoService;
        private readonly IProcessService _processService;
        #endregion

        #region Constructors
        public NetworkLicenseUsageViewModel(NetworkValidationResult networkValidationResult, ILicenseInfoService licenseInfoService, IProcessService processService)
        {
            Argument.IsNotNull(() => networkValidationResult);
            Argument.IsNotNull(() => licenseInfoService);
            Argument.IsNotNull(() => processService);

            _networkValidationResult = networkValidationResult;
            _licenseInfoService = licenseInfoService;
            _processService = processService;

            var assembly = AssemblyHelper.GetEntryAssembly();
            Title = assembly.Title() + " licence usage";

            PurchaseUrl = licenseInfoService.GetLicenseInfo().PurchaseUrl;
            CurrentUsers = networkValidationResult.CurrentUsers.ToList();
            MaximumNumberOfConcurrentUsages = networkValidationResult.MaximumConcurrentUsers;

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
    }
}