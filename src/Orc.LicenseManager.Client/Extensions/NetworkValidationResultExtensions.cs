namespace Orc.LicenseManager
{
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
            if (latestUser is not null)
            {
                return string.Equals(networkLicenseService.ComputerId, latestUser.ComputerId);
            }

            return false;
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
}
