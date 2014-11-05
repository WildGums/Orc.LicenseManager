// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkValidationResultExtensions.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System.Linq;
    using Catel;
    using Catel.IoC;
    using Models;
    using Services;

    public static class NetworkValidationResultExtensions
    {
        public static bool IsCurrentUserLatestUser(this NetworkValidationResult validationResult)
        {
            Argument.IsNotNull(() => validationResult);

            var serviceLocator = ServiceLocator.Default;
            var networkLicenseService = serviceLocator.ResolveType<INetworkLicenseService>();

            var latestUser = validationResult.GetLatestUser();
            if (latestUser != null)
            {
                return string.Equals(networkLicenseService.ComputerId, latestUser.ComputerId);
            }

            return false;
        }

        public static NetworkLicenseUsage GetLatestUser(this NetworkValidationResult validationResult)
        {
            Argument.IsNotNull(() => validationResult);

            var latestUsage = (from usage in validationResult.CurrentUsers
                               orderby usage.StartDateTime descending
                               select usage).FirstOrDefault();

            return latestUsage;
        }
    }
}