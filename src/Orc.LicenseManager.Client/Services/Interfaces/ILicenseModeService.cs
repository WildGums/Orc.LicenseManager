namespace Orc.LicenseManager
{
    using System.Collections.Generic;

    public interface ILicenseModeService
    {
        List<LicenseMode> GetAvailableLicenseModes();
        bool IsLicenseModeAvailable(LicenseMode licenseMode);
    }
}
