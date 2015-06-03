// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdentificationServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System.Threading.Tasks;
    using Catel.Threading;

    public static class IIdentificationServiceExtensions
    {
        public static Task<string> GetMachineIdAsync(this IIdentificationService identificationService)
        {
            return TaskHelper.Run(() => identificationService.GetMachineId());
        }
    }
}