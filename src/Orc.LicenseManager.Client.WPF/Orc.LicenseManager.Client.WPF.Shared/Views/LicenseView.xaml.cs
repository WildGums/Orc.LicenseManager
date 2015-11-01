// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseView.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.LicenseManager.Views
{
    using System.Windows;

    public sealed partial class LicenseView
    {
        public LicenseView()
        {
            InitializeComponent();
        }

        public bool ShowAbout
        {
            get { return (bool)GetValue(ShowAboutProperty); }
            set { SetValue(ShowAboutProperty, value); }
        }

        public static readonly DependencyProperty ShowAboutProperty = DependencyProperty.Register("ShowAbout", typeof(bool), 
            typeof(LicenseView), new PropertyMetadata(true));
    }
}