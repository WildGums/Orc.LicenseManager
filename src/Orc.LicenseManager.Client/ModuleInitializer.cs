using System;
using Catel.IoC;
using Catel.Reflection;
using Catel.Services;
using Catel.Services.Models;
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

        serviceLocator.RegisterType<ILicenseService, LicenseService>();
        serviceLocator.RegisterType<ISimpleLicenseService, SimpleLicenseService>();
        serviceLocator.RegisterType<INetworkLicenseService, NetworkLicenseService>();
        serviceLocator.RegisterType<IExpirationBehavior, PreventUsageOfAnyVersionExpirationBehavior>();

        serviceLocator.RegisterTypeIfNotYetRegistered<ILicenseVisualizerService, EmptyLicenseVisualizerService>();
    }
}