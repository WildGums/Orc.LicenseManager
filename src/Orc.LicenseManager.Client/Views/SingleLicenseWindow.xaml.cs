// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleLicenseWindow.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Views
{
    using Catel.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for SingleLicenseWindow.xaml.
    /// </summary>
    public partial class SingleLicenseWindow : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleLicenseWindow"/> class.
        /// </summary>
        public SingleLicenseWindow()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleLicenseWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public SingleLicenseWindow(SingleLicenseViewModel viewModel)
            : base(viewModel, DataWindowMode.OkCancel)
        {
            InitializeComponent();
        }
        #endregion
    }
}