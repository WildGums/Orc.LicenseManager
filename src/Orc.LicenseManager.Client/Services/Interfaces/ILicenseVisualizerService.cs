// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRequestLicenseService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    public interface ILicenseVisualizerService
    {
        /// <summary>
        /// Shows the single license dialog including all company info.
        /// </summary>
        /// <exception cref="System.Exception">Please use the Initialize method first</exception>
        void ShowLicense();
    }
}
