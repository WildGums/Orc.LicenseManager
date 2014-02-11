// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Client.Example
{
    using System;
    using System.Windows;
    using Catel.Reflection;
    using Catel.Windows;
    using Services;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Methods
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            Catel.Logging.LogManager.AddDebugListener(true);
#endif

            Console.WriteLine(typeof(ILicenseService));

            System.AppDomain.CurrentDomain.PreloadAssemblies();

            StyleHelper.CreateStyleForwardersForDefaultStyles();

            base.OnStartup(e);
        }
        #endregion
    }
}