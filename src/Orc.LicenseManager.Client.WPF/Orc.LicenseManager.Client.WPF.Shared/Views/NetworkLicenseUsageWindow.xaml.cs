// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseUsage.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Views
{
    using Catel.Windows;
    using ViewModels;

    public partial class NetworkLicenseUsageWindow
    {
        #region Constructors
        public NetworkLicenseUsageWindow()
            : this(null)
        {
        }

        public NetworkLicenseUsageWindow(NetworkLicenseUsageViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            InitializeComponent();
        }
        #endregion
    }
}