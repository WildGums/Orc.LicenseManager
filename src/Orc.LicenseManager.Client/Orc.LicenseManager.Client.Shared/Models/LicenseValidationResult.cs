// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseValidationResult.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Models
{
    public class LicenseValidationResult
    {
        public bool IsValid { get; set; }

        public string AdditionalInfo { get; set; }
    }
}