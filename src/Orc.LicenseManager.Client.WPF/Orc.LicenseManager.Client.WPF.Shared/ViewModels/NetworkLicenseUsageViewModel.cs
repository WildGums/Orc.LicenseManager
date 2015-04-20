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
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Models;

    public class NetworkLicenseUsageViewModel : ViewModelBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly NetworkValidationResult _networkValidationResult;
        #endregion

        #region Constructors
        public NetworkLicenseUsageViewModel(NetworkValidationResult networkValidationResult)
        {
            Argument.IsNotNull(() => networkValidationResult);

            _networkValidationResult = networkValidationResult;

            var assembly = AssemblyHelper.GetEntryAssembly();
            Title = assembly.Title() + " licence usage";

            CurrentUsers = networkValidationResult.CurrentUsers.ToList();
            MaximumNumberOfConcurrentUsages = networkValidationResult.MaximumConcurrentUsers;

            CloseApplication = new Command(OnCloseApplicationExecute);
            BuyLicenses = new Command(OnBuyLicensesExecute);
        }
        #endregion

        #region Properties
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
            Log.Info("Buying licenses");


        }
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            await base.Initialize();
        }

        protected override async Task Close()
        {
            await base.Close();
        }
        #endregion
    }
}