// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMachineLicenseValidationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using Catel.Data;

    public interface IMachineLicenseValidationService
    {
        IValidationContext Validate(string machineIdToValidate);
        int Threshold { get; set; }
    }
}