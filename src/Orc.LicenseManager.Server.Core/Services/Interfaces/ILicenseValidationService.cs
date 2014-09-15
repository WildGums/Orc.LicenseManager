// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseValidationService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Services
{
    using System.Threading.Tasks;

    public interface ILicenseValidationService
    {
        #region Methods
        Task<bool> ValidateLicense(string license);
        #endregion
    }
}