namespace Orc.LicenseManager.Server.Models
{
    public class LicenseValidationResult
    {
        public LicenseValidationResult(bool isValid, string additionalInfo)
        {
            IsValid = isValid;
            AdditionalInfo = additionalInfo;
        }

        public bool IsValid { get; private set; }

        public string AdditionalInfo { get; private set; }
    }
}
