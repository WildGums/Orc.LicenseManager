namespace Orc.LicenseManager;

using System;
using System.IO;
using Catel.Logging;
using Catel.Reflection;
using Catel.Services;
using FileSystem;
using Microsoft.Extensions.Logging;

public class LicenseLocationService : ILicenseLocationService
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(LicenseLocationService));

    private readonly IApplicationIdService _applicationIdService;
    private readonly IFileService _fileService;
    private readonly IAppDataService _appDataService;

    public LicenseLocationService(IApplicationIdService applicationIdService, 
        IFileService fileService, IAppDataService appDataService)
    {
        _applicationIdService = applicationIdService;
        _fileService = fileService;
        _appDataService = appDataService;
    }

    public string? LoadLicense(LicenseMode licenseMode)
    {
        try
        {
            var fileName = GetLicenseLocation(licenseMode);
            if (!string.IsNullOrWhiteSpace(fileName) && _fileService.Exists(fileName))
            {
                Logger.LogDebug("Loading license from '{FileName}'", fileName);

                return _fileService.ReadAllText(fileName);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load license for license mode '{LicenseMode}'", licenseMode);
        }

        return null;
    }

    public virtual string? GetLicenseLocation(LicenseMode licenseMode)
    {
        try
        {
            var entryAssembly = AssemblyHelper.GetRequiredEntryAssembly();

            var companyName = _applicationIdService.CompanyName;
            if (string.IsNullOrWhiteSpace(companyName))
            {
                companyName = entryAssembly.Company() ?? string.Empty;
            }

            var productName = _applicationIdService.ProductName;
            if (string.IsNullOrWhiteSpace(productName))
            {
                productName = entryAssembly.Product() ?? string.Empty;
            }

            if (licenseMode == LicenseMode.CurrentUser)
            {
                return Path.Combine(_appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "LicenseInfo.xml");
            }

            // Keep using static path
            return Path.Combine(Catel.IO.Path.GetApplicationDataDirectoryForAllUsers(companyName, productName), "LicenseInfo.xml");
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to get license location for license mode '{LicenseMode}', probably access is denied", licenseMode);
            return null;
        }
    }
}
