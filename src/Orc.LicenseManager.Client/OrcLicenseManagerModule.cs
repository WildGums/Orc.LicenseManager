namespace Orc;

using Catel.Services;
using Catel.ThirdPartyNotices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orc.LicenseManager;

/// <summary>
/// Core module which allows the registration of default services in the service collection.
/// </summary>
public static class OrcLicenseManagerModule
{
    public static IServiceCollection AddOrcLicenseManager(this IServiceCollection serviceCollection)
    {
        // Must be add, not add singleton
        serviceCollection.AddSingleton<INetworkValidationHandler, NetworkValidationHandler>();

        serviceCollection.TryAddSingleton<IApplicationIdService, ApplicationIdService>();
        serviceCollection.TryAddSingleton<ILicenseService, LicenseService>();
        serviceCollection.TryAddSingleton<ILicenseLocationService, LicenseLocationService>();
        serviceCollection.TryAddSingleton<ILicenseModeService, LicenseModeService>();
        serviceCollection.TryAddSingleton<ILicenseValidationService, LicenseValidationService>();
        serviceCollection.TryAddSingleton<IMachineLicenseValidationService, MachineLicenseValidationService>();
        serviceCollection.TryAddSingleton<ISimpleLicenseService, SimpleLicenseService>();
        serviceCollection.TryAddSingleton<INetworkLicenseService, NetworkLicenseService>();
        serviceCollection.TryAddSingleton<IExpirationBehavior, PreventUsageOfAnyVersionExpirationBehavior>();
        serviceCollection.TryAddSingleton<IIdentificationService, IdentificationService>();

        serviceCollection.TryAddSingleton<ILicenseVisualizerService, EmptyLicenseVisualizerService>();

        serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.LicenseManager.Client", "Orc.LicenseManager.Properties", "Resources"));

        serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new LibraryThirdPartyNotice("Orc.LicenseManager", "https://github.com/wildgums/orc.licensemanager"));
        serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("Portable.Licensing", "https://github.com/dnauck/Portable.Licensing", "Orc.LicenseManager.Client", "Orchestra", "Resources.ThirdPartyNotices.portable.licensing.txt"));

        return serviceCollection;
    }
}
