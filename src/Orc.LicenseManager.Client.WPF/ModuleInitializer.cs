using System;
using System.Windows;
using Catel.IoC;
using Catel.Reflection;
using Catel.Services;
using Catel.Services.Models;
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
        // Force loading of Catel libraries
        AppDomain.CurrentDomain.PreloadAssemblies();

        Console.WriteLine(typeof (Catel.MVVM.ViewModelBase));

        var serviceLocator = ServiceLocator.Default;
        serviceLocator.RegisterType<ILicenseService, LicenseService>();
        serviceLocator.RegisterType<ISimpleLicenseService, SimpleLicenseService>();

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.LicenseManager.Client.WPF", "Orc.LicenseManager.Properties", "Resources"));
    }
}