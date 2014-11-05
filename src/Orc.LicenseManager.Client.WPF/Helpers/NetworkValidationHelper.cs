// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkValidationHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System.Diagnostics;
    using System.Windows;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Reflection;
    using Catel.Services;
    using Services;

    public static class NetworkValidationHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static async void DefaultNetworkLicenseServiceValidationHandler(object sender, NetworkValidatedEventArgs e)
        {
            var validationResult = e.ValidationResult;
            if (!validationResult.IsValid)
            {
                if (validationResult.IsCurrentUserLatestUser())
                {
                    var dependencyResolver = IoCConfiguration.DefaultDependencyResolver;
                    var messageService = dependencyResolver.Resolve<IMessageService>();

                    var entryAssembly = AssemblyHelper.GetEntryAssembly();

                    var message = string.Format("The current number of usages for {0} is higher than the maximum number of concurrent users allowed based on the current license. Since this computer is the last one using the license, the software has to shut down.\n\nIf you feel that you have not reached the maximum number of usages, please contact support.\n\nThe maximum allowed is {1}, the current usage is {2}.", entryAssembly.Title(), validationResult.MaximumConcurrentUsers, validationResult.CurrentUsers.Count);

                    Log.Error(message);

                    Log.Error("Listing all the usages of the license:");
                    Log.Indent();

                    foreach (var licenseUsage in validationResult.CurrentUsers)
                    {
                        Log.Error("* {0}", licenseUsage);
                    }

                    Log.Unindent();

                    await messageService.ShowError(message);

                    Log.Info("Shutting down application");

                    var process = Process.GetCurrentProcess();
                    process.Kill();
                }
            }
        }
    }
}