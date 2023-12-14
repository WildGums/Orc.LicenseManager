namespace Orc.LicenseManager;

using System;
using System.Linq;
using Catel.IoC;

public static class NetworkValidationResultExtensions
{
    public static bool IsCurrentUserLatestUser(this NetworkValidationResult validationResult)
    {
        ArgumentNullException.ThrowIfNull(validationResult);

        var serviceLocator = ServiceLocator.Default;
        var networkLicenseService = serviceLocator.ResolveRequiredType<INetworkLicenseService>();

        var latestUser = validationResult.GetLatestUser();
        return latestUser is not null && string.Equals(networkLicenseService.ComputerId, latestUser.ComputerId);
    }

    public static NetworkLicenseUsage? GetLatestUser(this NetworkValidationResult validationResult)
    {
        ArgumentNullException.ThrowIfNull(validationResult);

        var latestUsage = (from usage in validationResult.CurrentUsers
            orderby usage.StartDateTime descending
            select usage).FirstOrDefault();

        return latestUsage;
    }
}
