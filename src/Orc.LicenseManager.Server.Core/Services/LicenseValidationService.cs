// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseValidationService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Services
{
    using System.Threading.Tasks;
    using Catel;

    public class LicenseValidationService : ILicenseValidationService
    {
        public LicenseValidationService()
        {
            
        }

        public async Task<bool> ValidateLicense(string license)
        {
            Argument.IsNotNullOrWhitespace(() => license);

            
            // TODO: Verify license

            return true;
        }
    }
}