using Catel.IoC;
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
        serviceLocator.RegisterType<ILicenseValidationService, LicenseValidationService>();
        serviceLocator.RegisterType<ISimpleLicenseService, SimpleLicenseService>();
        serviceLocator.RegisterType<INetworkLicenseService, NetworkLicenseService>();
        serviceLocator.RegisterType<IExpirationBehavior, PreventUsageOfAnyVersionExpirationBehavior>();
        serviceLocator.RegisterType<IIdentificationService, IdentificationService>();

        serviceLocator.RegisterTypeIfNotYetRegistered<ILicenseVisualizerService, EmptyLicenseVisualizerService>();
    }
}