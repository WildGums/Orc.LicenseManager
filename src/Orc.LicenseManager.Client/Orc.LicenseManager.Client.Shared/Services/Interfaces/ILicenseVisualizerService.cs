// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRequestLicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Threading.Tasks;

    public interface ILicenseVisualizerService
    {
        /// <summary>
        /// Shows the single license dialog including all company info.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exception cref="System.Exception">Please use the Initialize method first</exception>
        /// <exception cref="Exception">The <see cref="LicenseService.Initialize" /> method must be run first.</exception>
        Task ShowLicense();
    }
}