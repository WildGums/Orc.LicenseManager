// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseView.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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