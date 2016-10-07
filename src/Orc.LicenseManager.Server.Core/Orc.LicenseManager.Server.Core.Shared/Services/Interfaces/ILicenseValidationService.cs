// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseValidationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface ILicenseValidationService
    {
        #region Methods
        Task<LicenseValidationResult> ValidateLicense(string license);
        #endregion
    }
}