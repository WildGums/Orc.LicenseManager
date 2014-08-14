// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowExtensions.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;
    using Catel;
    using Catel.Logging;

    /// <summary>
    /// Extensions for the window class.
    /// </summary>
    public static class WindowExtensions
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Applies the icon from the entry assembly (the application) to the window.
        /// </summary>
        /// <param name="window">The window.</param>
        public static void ApplyIconFromApplication(this Window window)
        {
            Argument.IsNotNull(() => window);

            try
            {
                if (window.Icon != null)
                {
                    return;
                }

                var currentApplication = Application.Current;
                if (currentApplication != null)
                {
                    var entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        var icon = Icon.ExtractAssociatedIcon(entryAssembly.Location);
                        if (icon != null)
                        {
                            window.Icon = Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
                                new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to set the application icon to the window");
            }
        }
    }
}