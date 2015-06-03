// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INetworkLicenseServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Threading.Tasks;
    using Catel.Threading;

    public static class INetworkLicenseServiceExtensions
    {
        public static Task InitializeAsync(this INetworkLicenseService networkLicenseService, TimeSpan pollingInterval = default(TimeSpan))
        {
            return TaskHelper.Run(() => networkLicenseService.Initialize(pollingInterval));
        }
    }
}