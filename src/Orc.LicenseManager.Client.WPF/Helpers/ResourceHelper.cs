// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;
    using System.Windows;

    internal static class ResourceHelper
    {
        #region Constants
        private static bool InitializedStyles = false;
        #endregion

        #region Methods
        public static void EnsureStyles()
        {
            if (InitializedStyles)
            {
                return;
            }

            var app = System.Windows.Application.Current;
            if (app == null)
            {
                return;
            }

            var resourceDictionary = new ResourceDictionary() {Source = new Uri("/Orc.LicenseManager.Client.WPF;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)};
            app.Resources.MergedDictionaries.Add(resourceDictionary);

            InitializedStyles = true;
        }
        #endregion
    }
}