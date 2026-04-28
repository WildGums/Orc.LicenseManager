namespace Orc.LicenseManager;

using System;
using System.Collections.Generic;
using Catel.Logging;
using FileSystem;
using Microsoft.Extensions.Logging;

public class LicenseModeService : ILicenseModeService
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(LicenseModeService));

    private readonly IFileService _fileService;
    private readonly ILicenseLocationService _licenseLocationService;

    public LicenseModeService(IFileService fileService, ILicenseLocationService licenseLocationService)
    {
        ArgumentNullException.ThrowIfNull(fileService);
        ArgumentNullException.ThrowIfNull(licenseLocationService);

        _fileService = fileService;
        _licenseLocationService = licenseLocationService;
    }

    public IReadOnlyList<LicenseMode> GetAvailableLicenseModes()
    {
        var licenseModes = new List<LicenseMode>();

        if (IsLicenseModeAvailable(LicenseMode.CurrentUser))
        {
            licenseModes.Add(LicenseMode.CurrentUser);
        }

        if (IsLicenseModeAvailable(LicenseMode.MachineWide))
        {
            licenseModes.Add(LicenseMode.MachineWide);
        }

        return licenseModes;
    }

    public bool IsLicenseModeAvailable(LicenseMode licenseMode)
    {
        var licenseLocation = _licenseLocationService.GetLicenseLocation(licenseMode);
        if (string.IsNullOrWhiteSpace(licenseLocation))
        {
            return false;
        }

        try
        {
            var checkLocation = $"{licenseLocation}.tmp";
            if (!_fileService.CanOpenWrite(checkLocation))
            {
                return false;
            }

            _fileService.Delete(checkLocation);

            return true;
        }
        catch (Exception ex)
        {
            Logger.LogDebug(ex, $"Failed to access location @ '{licenseLocation}', assuming license mode '{licenseMode}' is not available");
            return false;
        }
    }
}
