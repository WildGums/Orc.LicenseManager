using Catel.IoC;
using Catel.Services;
using Orc.LicenseManager;
using Orc.LicenseManager.Services;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterType<IApplicationIdService, ApplicationIdService>();
        serviceLocator.RegisterType<ILicenseService, LicenseService>();
        serviceLocator.RegisterType<ILicenseLocationService, LicenseLocationService>();
        serviceLocator.RegisterType<ILicenseModeService, LicenseModeService>();
        serviceLocator.RegisterType<ILicenseValidationService, LicenseValidationService>();
        serviceLocator.RegisterType<IMachineLicenseValidationService, MachineLicenseValidationService>();
        serviceLocator.RegisterType<ISimpleLicenseService, SimpleLicenseService>();
        serviceLocator.RegisterType<INetworkLicenseService, NetworkLicenseService>();
        serviceLocator.RegisterType<IExpirationBehavior, PreventUsageOfAnyVersionExpirationBehavior>();
        serviceLocator.RegisterType<IIdentificationService, IdentificationService>();

        serviceLocator.RegisterTypeIfNotYetRegistered<ILicenseVisualizerService, EmptyLicenseVisualizerService>();

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.LicenseManager.Client", "Orc.LicenseManager.Properties", "Resources"));
    }
}