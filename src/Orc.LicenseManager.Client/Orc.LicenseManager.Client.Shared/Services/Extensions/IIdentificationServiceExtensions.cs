// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdentificationServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System.Threading.Tasks;

    public static class IIdentificationServiceExtensions
    {
        public static async Task<string> GetMachineIdAsync(this IIdentificationService identificationService)
        {
            return await Task.Factory.StartNew(() => identificationService.GetMachineId());
        }
    }
}