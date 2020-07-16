// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerLicenseValidation.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    public class ServerLicenseValidation
    {
        #region Properties
        public string MachineId { get; set; }

        public string ProductName { get; set; }

        public string ProductVersion { get; set; }

        public string License { get; set; }
        #endregion
    }
}
