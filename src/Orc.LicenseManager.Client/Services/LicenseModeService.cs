namespace Orc.LicenseManager
{
    using System;
    using System.Collections.Generic;
    using Catel;
    using Catel.Logging;
    using FileSystem;

    public class LicenseModeService : ILicenseModeService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IFileService _fileService;
        private readonly ILicenseLocationService _licenseLocationService;

        public LicenseModeService(IFileService fileService, ILicenseLocationService licenseLocationService)
        {
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => licenseLocationService);

            _fileService = fileService;
            _licenseLocationService = licenseLocationService;
        }

        public List<LicenseMode> GetAvailableLicenseModes()
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
                Log.Debug(ex, $"Failed to access location @ '{licenseLocation}', assuming license mode '{licenseMode}' is not available");
                return false;
            }
        }
    }
}
