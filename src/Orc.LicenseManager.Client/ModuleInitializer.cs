using System;
using Catel.IoC;
using Catel.Reflection;
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

        var servicelocator = ServiceLocator.Default;
        servicelocator.RegisterType<ILicenseService, LicenseService>();
        
    }
}