// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Client.Example
{
    using System;
    using System.Globalization;
    using System.Windows;
    using Catel.IO;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Reflection;
    using Catel.Services;
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

            var logPath = Path.Combine(GetType().Assembly.GetDirectory(), "debug.log");
            LogManager.AddListener(new FileLogListener(logPath, 25 * 1024));
#endif

            //var consoleLogListener = new ConsoleLogListener();
            //consoleLogListener.IgnoreCatelLogging = true;
            //LogManager.AddListener(consoleLogListener);

            Console.WriteLine(typeof(ILicenseService));

            StyleHelper.CreateStyleForwardersForDefaultStyles();

            var serviceLocator = ServiceLocator.Default;
            var languageService = serviceLocator.ResolveType<ILanguageService>();

            //languageService.PreferredCulture = new CultureInfo("nl-NL");
            //languageService.FallbackCulture = new CultureInfo("en-US");

            languageService.PreloadLanguageSources();

            base.OnStartup(e);
        }
        #endregion
    }
}