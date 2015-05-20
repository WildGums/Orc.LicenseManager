// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INetworkLicenseServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Threading.Tasks;

    public static class INetworkLicenseServiceExtensions
    {
        public static async Task InitializeAsync(this INetworkLicenseService networkLicenseService, TimeSpan pollingInterval = default(TimeSpan))
        {
            await Task.Factory.StartNew(() => networkLicenseService.Initialize(pollingInterval));
        }
    }
}