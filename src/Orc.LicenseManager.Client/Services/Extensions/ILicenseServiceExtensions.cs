namespace Orc.LicenseManager;

using System;

public static class ILicenseServiceExtensions
{
    public static DateTime? GetCurrentLicenseExpirationDateTime(this ILicenseService licenseService)
    {
        ArgumentNullException.ThrowIfNull(licenseService);

        var license = licenseService.CurrentLicense;
        return license?.Expiration;
    }

    public static bool AnyExistingLicense(this ILicenseService licenseService)
    {
        return licenseService.LicenseExists() || licenseService.LicenseExists(LicenseMode.MachineWide);
    }

    public static string? LoadExistingLicense(this ILicenseService licenseService)
    {
        string? licenseString = null;

        try
        {
            licenseString = licenseService.LoadLicense();
            if (string.IsNullOrWhiteSpace(licenseString))
            {
                licenseString = licenseService.LoadLicense(LicenseMode.MachineWide);
            }
        }
        catch (Exception)
        {
            // Tolerated
        }

        return licenseString;
    }
}
