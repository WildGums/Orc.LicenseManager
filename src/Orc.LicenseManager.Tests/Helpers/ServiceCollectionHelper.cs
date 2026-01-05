namespace Orc.LicenseManager.Tests
{
    using Catel;
    using Microsoft.Extensions.DependencyInjection;

    internal static class ServiceCollectionHelper
    {
        public static IServiceCollection CreateServiceCollection()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging();
            serviceCollection.AddCatelCore();
            serviceCollection.AddCatelMvvm();
            serviceCollection.AddOrcLicenseManager();
            serviceCollection.AddOrcLicenseManagerXaml();

            return serviceCollection;
        }
    }
}
