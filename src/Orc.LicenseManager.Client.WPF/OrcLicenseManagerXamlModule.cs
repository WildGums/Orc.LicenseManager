namespace Orc;

using Catel.IoC;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orc.LicenseManager;
using Orc.LicenseManager.ViewModels;
using Orc.LicenseManager.Views;

/// <summary>
/// Core module which allows the registration of default services in the service collection.
/// </summary>
public static class OrcLicenseManagerXamlModule
{
    public static IServiceCollection AddOrcLicenseManagerXaml(this IServiceCollection serviceCollection)
    {
        // Must be add (to overwrite)
        serviceCollection.AddSingleton<INetworkValidationHandler, WpfNetworkValidationHandler>();

        // Must be add (to overwrite)
        serviceCollection.AddSingleton<ILicenseVisualizerService, DialogLicenseVisualizerService>();

        serviceCollection.TryAddSingleton<UIVisualizerInitializer>();

        serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.LicenseManager.Client.WPF", "Orc.LicenseManager.Properties", "Resources"));

        return serviceCollection;
    }

    private class UIVisualizerInitializer : IConstructAtStartup
    {
        public UIVisualizerInitializer(IUIVisualizerService uiVisualizerService)
        {
            uiVisualizerService.Register<LicenseViewModel, LicenseWindow>();
        }
    }
}
