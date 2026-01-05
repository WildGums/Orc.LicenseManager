namespace Orc.LicenseManager;

using System;
using System.Linq;

public static class NetworkValidationResultExtensions
{
    public static bool IsCurrentUserLatestUser(this INetworkLicenseService networkLicenseService, 
        NetworkValidationResult validationResult)
    {
        ArgumentNullException.ThrowIfNull(validationResult);

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
