// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkValidationResultExtensions.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System.Linq;
    using Catel.IoC;
    using Models;
    using Services;

    public static class NetworkValidationResultExtensions
    {
        public static bool IsCurrentUserLatestUser(this NetworkValidationResult validationResult)
        {
            var serviceLocator = ServiceLocator.Default;
            var networkLicenseService = serviceLocator.ResolveType<INetworkLicenseService>();

            var lastUser = (from usage in validationResult.CurrentUsers
                            orderby usage.StartDateTime descending
                            select usage).FirstOrDefault();
            if (lastUser != null)
            {
                return string.Equals(networkLicenseService.ComputerId, lastUser.ComputerId);
            }

            return false;
        }
    }
}