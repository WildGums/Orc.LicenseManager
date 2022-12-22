namespace Orc.LicenseManager
{
    using System.Diagnostics;
    using Catel.Logging;
    using Catel.Reflection;

    public static class NetworkValidationHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static bool IsInErrorHandling;

        public static void DefaultNetworkLicenseServiceValidationHandler(object? sender, NetworkValidatedEventArgs e)
        {
            if (IsInErrorHandling)
            {
                Log.Warning("Already handling the invalid license usage");
                return;
            }

            var validationResult = e.ValidationResult;
            if (!validationResult.IsValid && validationResult.IsCurrentUserLatestUser())
            {
                IsInErrorHandling = true;

                var entryAssembly = AssemblyHelper.GetRequiredEntryAssembly();

                var message = string.Format(
                    "The current number of usages for {0} is higher than the maximum number of concurrent users allowed based on the current license. Since this computer is the last one using the license, the software has to shut down.\n\nIf you feel that you have not reached the maximum number of usages, please contact support.\n\nThe maximum allowed is {1}, the current usage is {2}.", entryAssembly.Title(), validationResult.MaximumConcurrentUsers, validationResult.CurrentUsers.Count);

                Log.Error(message);

                Log.Error("Listing all the usages of the license:");
                Log.Indent();

                foreach (var licenseUsage in validationResult.CurrentUsers)
                {
                    Log.Error("* {0}", licenseUsage);
                }

                Log.Unindent();

                Log.Info("Shutting down application");

                var process = Process.GetCurrentProcess();
                process.Kill();
            }
        }
    }
}
