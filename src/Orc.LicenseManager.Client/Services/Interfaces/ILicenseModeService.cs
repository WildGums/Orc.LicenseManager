namespace Orc.LicenseManager;

using System.Collections.Generic;

public interface ILicenseModeService
{
    IReadOnlyList<LicenseMode> GetAvailableLicenseModes();
    bool IsLicenseModeAvailable(LicenseMode licenseMode);
}
