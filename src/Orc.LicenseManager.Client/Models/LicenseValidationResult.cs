// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseValidationResult.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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