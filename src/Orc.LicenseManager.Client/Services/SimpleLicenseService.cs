namespace Orc.LicenseManager;

using System;
using System.Threading.Tasks;
using Catel.Logging;

/// <summary>
/// Simple license service.
/// </summary>
public class SimpleLicenseService : ISimpleLicenseService
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly ILicenseService _licenseService;
    private readonly ILicenseValidationService _licenseValidationService;
    private readonly ILicenseVisualizerService _licenseVisualizerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleLicenseService" /> class.
    /// </summary>
    /// <param name="licenseService">The license service.</param>
    /// <param name="licenseValidationService">The license validation service.</param>
    /// <param name="licenseVisualizerService">The license visualizer service.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="licenseService" /> is <c>null</c>.</exception>
    public SimpleLicenseService(ILicenseService licenseService, ILicenseValidationService licenseValidationService, 
        ILicenseVisualizerService licenseVisualizerService)
    {
        ArgumentNullException.ThrowIfNull(licenseService);
        ArgumentNullException.ThrowIfNull(licenseValidationService);
        ArgumentNullException.ThrowIfNull(licenseVisualizerService);

        _licenseService = licenseService;
        _licenseValidationService = licenseValidationService;
        _licenseVisualizerService = licenseVisualizerService;
    }

    /// <summary>
    /// Validates the license on the server. This method is the same as <see cref="ValidateAsync" /> but also checks the server if the license
    /// is valid.
    /// </summary>
    /// <param name="serverUrl">The server URL.</param>
    /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
    public async Task<bool> ValidateOnServerAsync(string serverUrl)
    {
        if (!EnsureLicenseExists())
        {
            return false;
        }

        var licenseString = _licenseService.LoadExistingLicense();
        if (string.IsNullOrWhiteSpace(licenseString))
        {
            return false;
        }

        // Server first so it's possible to make licenses invalid
        var licenseValidationResult = await _licenseValidationService.ValidateLicenseOnServerAsync(licenseString, serverUrl);
        if (!licenseValidationResult.IsValid)
        {
            Log.Error("The server returned that the license is invalid and contains the following errors:");
            Log.Error("  * {0}", licenseValidationResult.AdditionalInfo);

            return false;
        }

        Log.Debug("Server returned valid license, doing a local check to be sure that the server wasn't forged");

        var validationContext = await _licenseValidationService.ValidateLicenseAsync(licenseString);
        if (validationContext.HasErrors)
        {
            Log.Error("The license is invalid and contains the following errors:");
            foreach (var error in validationContext.GetErrors())
            {
                Log.Error("  * {0}", error.Message);
            }

            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates the license in a very simple manner. This method is wrapper around the <see cref="ILicenseService" />.
    /// </summary>
    /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
    /// <remarks>Note that this method might show a dialog so must be run on the UI thread.</remarks>
    public async Task<bool> ValidateAsync()
    {
        if (!EnsureLicenseExists())
        {
            return false;
        }

        var licenseString = _licenseService.LoadExistingLicense();
        if (string.IsNullOrWhiteSpace(licenseString))
        {
            return false;
        }

        var licenseValidation = await _licenseValidationService.ValidateLicenseAsync(licenseString);

        return !licenseValidation.HasErrors;
    }

    private bool EnsureLicenseExists()
    {
        if (!_licenseService.AnyExistingLicense())
        {
            _licenseVisualizerService.ShowLicense();
        }

        return _licenseService.AnyExistingLicense();
    }
}