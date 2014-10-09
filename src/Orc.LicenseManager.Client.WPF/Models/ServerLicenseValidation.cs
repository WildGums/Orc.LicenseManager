// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerLicenseValidation.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Models
{
    public class ServerLicenseValidation
    {
        #region Properties
        public string ProductName { get; set; }

        public string ProductVersion { get; set; }

        public string License { get; set; }
        #endregion
    }
}