namespace Orc.LicenseManager;

using System.Diagnostics;
using Catel.Logging;
using Catel.Reflection;
using Microsoft.Extensions.Logging;

public class NetworkValidationHandler : INetworkValidationHandler
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(NetworkValidationHandler));

    private readonly INetworkLicenseService _networkLicenseService;

    private bool _isInErrorHandling;

    public NetworkValidationHandler(INetworkLicenseService networkLicenseService)
    {
        _networkLicenseService = networkLicenseService;
    }

    public void HandleNetworkValidation(object? sender, NetworkValidatedEventArgs e)
    {
        if (_isInErrorHandling)
        {
            Logger.LogWarning("Already handling the invalid license usage");
            return;
        }

        var validationResult = e.ValidationResult;
        if (validationResult.IsValid ||
            !_networkLicenseService.IsCurrentUserLatestUser(validationResult))
        {
            return;
        }

        _isInErrorHandling = true;

        var entryAssembly = AssemblyHelper.GetRequiredEntryAssembly();

        var message = $"The current number of usages for {entryAssembly.Title()} is higher than the maximum number of concurrent users allowed based on the current license. Since this computer is the last one using the license, the software has to shut down.\n\nIf you feel that you have not reached the maximum number of usages, please contact support.\n\nThe maximum allowed is {validationResult.MaximumConcurrentUsers}, the current usage is {validationResult.CurrentUsers.Count}.";

        Logger.LogError(message);

        Logger.LogError("Listing all the usages of the license:");

        foreach (var licenseUsage in validationResult.CurrentUsers)
        {
            Logger.LogError("  * {0}", licenseUsage);
        }

        Logger.LogInformation("Shutting down application");

        var process = Process.GetCurrentProcess();
        process.Kill();
    }
}
