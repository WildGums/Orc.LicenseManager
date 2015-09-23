// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMachineLicenseValidationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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