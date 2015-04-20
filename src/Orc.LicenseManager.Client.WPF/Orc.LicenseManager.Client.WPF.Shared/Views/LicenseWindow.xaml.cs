// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleLicenseWindow.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Views
{
    using Catel;
    using Catel.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for SingleLicenseWindow.xaml.
    /// </summary>
    public partial class LicenseWindow : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseWindow"/> class.
        /// </summary>
        public LicenseWindow()
            : this(null)
        {
        }

        /// <summary>5
        /// Initializes a new instance of the <see cref="LicenseWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public LicenseWindow(LicenseViewModel viewModel)
            : base(viewModel, DataWindowMode.OkCancel)
        {
            InitializeComponent();

            if (CatelEnvironment.IsInDesignMode)
            {
                return;
            }

            LicenseManager.ResourceHelper.EnsureStyles();

            this.ApplyIconFromApplication();

            this.RemoveCloseButton();
        }
        #endregion
    }
}