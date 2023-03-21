namespace Orc.LicenseManager;

using Catel.Data;

public interface IMachineLicenseValidationService
{
    IValidationContext Validate(string machineIdToValidate);
    int Threshold { get; set; }
}