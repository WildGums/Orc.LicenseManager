namespace Orc.LicenseManager.Server.Models;

public class LicenseValidationResult
{
    public LicenseValidationResult(bool isValid, string additionalInfo)
    {
        IsValid = isValid;
        AdditionalInfo = additionalInfo;
    }

    public bool IsValid { get; }

    public string AdditionalInfo { get; }
}
