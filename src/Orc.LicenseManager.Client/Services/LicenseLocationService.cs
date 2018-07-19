// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseLocationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.IO;
    using Catel;
    using Catel.Logging;
    using Catel.Reflection;
    using FileSystem;

    public class LicenseLocationService : ILicenseLocationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IApplicationIdService _applicationIdService;
        private readonly IFileService _fileService;

        public LicenseLocationService(IApplicationIdService applicationIdService, IFileService fileService)
        {
            Argument.IsNotNull(() => applicationIdService);
            Argument.IsNotNull(() => fileService);

            _applicationIdService = applicationIdService;
            _fileService = fileService;
        }

        public string LoadLicense(LicenseMode licenseMode)
        {
            try
            {
                var fileName = GetLicenseLocation(licenseMode);
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    Log.Debug("Loading license from '{0}'", fileName);

                    return _fileService.ReadAllText(fileName);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to load license for license mode '{licenseMode}'");
            }

            return null;
        }

        public virtual string GetLicenseLocation(LicenseMode licenseMode)
        {
            try
            {
                var entryAssembly = AssemblyHelper.GetEntryAssembly();

                var companyName = _applicationIdService.CompanyName;
                if (string.IsNullOrWhiteSpace(companyName))
                {
                    companyName = entryAssembly.Company();
                }

                var productName = _applicationIdService.ProductName;
                if (string.IsNullOrWhiteSpace(productName))
                {
                    productName = entryAssembly.Product();
                }

                if (licenseMode == LicenseMode.CurrentUser)
                {
                    return Path.Combine(Catel.IO.Path.GetApplicationDataDirectory(companyName, productName), "LicenseInfo.xml");
                }

                return Path.Combine(Catel.IO.Path.GetApplicationDataDirectoryForAllUsers(companyName, productName), "LicenseInfo.xml");
            }
            catch (Exception ex)
            {
                Log.Warning(ex, $"Failed to get license location for license mode '{licenseMode}', probably access is denied");
                return null;
            }
        }
    }
}