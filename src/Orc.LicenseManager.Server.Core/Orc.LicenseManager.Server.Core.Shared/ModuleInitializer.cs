using Catel.IoC;
using Orc.LicenseManager.Server;
using Orc.LicenseManager.Server.Services;

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

        RepositoryInitializer.RegisterRepositories();

        serviceLocator.RegisterType<IAccountService, AccountService>();
        serviceLocator.RegisterType<ILicenseGenerationService, LicenseGenerationGenerationService>();
        serviceLocator.RegisterType<ILicenseValidationService, LicenseValidationService>();
    }
}