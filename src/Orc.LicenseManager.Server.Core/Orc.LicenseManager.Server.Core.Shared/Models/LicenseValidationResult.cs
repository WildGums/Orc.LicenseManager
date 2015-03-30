// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseValidationResult.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Models
{
    public class LicenseValidationResult
    {
        #region Constructors
        public LicenseValidationResult(bool isValid, string additionalInfo)
        {
            IsValid = isValid;
            AdditionalInfo = additionalInfo;
        }
        #endregion

        #region Properties
        public bool IsValid { get; private set; }

        public string AdditionalInfo { get; private set; }
        #endregion
    }
}