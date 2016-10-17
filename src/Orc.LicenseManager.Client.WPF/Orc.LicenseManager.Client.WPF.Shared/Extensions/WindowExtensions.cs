// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
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
                            window.SetCurrentValue(Window.IconProperty, Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
                                new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to set the application icon to the window");
            }
        }

        /// <summary>
        /// Removes the close button from the specified window.
        /// </summary>
        /// <param name="window">The window.</param>
        public static void RemoveCloseButton(this Window window)
        {
            if (!window.IsVisible)
            {
                window.SourceInitialized += OnWindowInitializedForRemoveCloseButton;
                return;
            }

            var windowInteropHelper = new WindowInteropHelper(window);
            var style = GetWindowLong(windowInteropHelper.Handle, GWL_STYLE);

            SetWindowLong(windowInteropHelper.Handle, GWL_STYLE, style & ~WS_SYSMENU);
        }

        private static void OnWindowInitializedForRemoveCloseButton(object sender, EventArgs e)
        {
            var window = (Window) sender;
            window.SourceInitialized -= OnWindowInitializedForRemoveCloseButton;

            RemoveCloseButton(window);
        }

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x00080000;

        [DllImport("user32.dll")]
        private extern static int SetWindowLong(IntPtr hwnd, int index, int value);

        [DllImport("user32.dll")]
        private extern static int GetWindowLong(IntPtr hwnd, int index);
    }
}