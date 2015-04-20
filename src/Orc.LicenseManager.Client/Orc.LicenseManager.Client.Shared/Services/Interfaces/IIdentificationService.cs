// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdentificationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    public interface IIdentificationService
    {
        #region Methods
        string GetMachineId();
        #endregion
    }
}