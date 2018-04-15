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

        // TODO: Pick the right expiration behavior
        //serviceLocator.RegisterType<IExpirationBehavior, PreventUsageOfAnyVersionExpirationBehavior>();
        serviceLocator.RegisterType<IExpirationBehavior, PreventUsageOfLaterReleasedVersionsExpirationBehavior>();
        serviceLocator.RegisterType<ILicenseInfoService, LicenseInfoService>();

        // A Valid license for this application ID aka Public Key can be found in data/FakeLicenseInfo.txt
        var applicationIdService = serviceLocator.ResolveType<IApplicationIdService>(); 
        applicationIdService.ApplicationId = "MIIBKjCB4wYHKoZIzj0CATCB1wIBATAsBgcqhkjOPQEBAiEA/////wAAAAEAAAAAAAAAAAAAAAD///////////////8wWwQg/////wAAAAEAAAAAAAAAAAAAAAD///////////////wEIFrGNdiqOpPns+u9VXaYhrxlHQawzFOw9jvOPD4n0mBLAxUAxJ02CIbnBJNqZnjhE50mt4GffpAEIQNrF9Hy4SxCR/i85uVjpEDydwN9gS3rM6D0oTlF2JjClgIhAP////8AAAAA//////////+85vqtpxeehPO5ysL8YyVRAgEBA0IABHGri0/ra3c4Fi+x0pLZG0ZGPeVxOps9Lz6CY1IKMf9SKsmXNzWrUh0dPEU8qeOe4DNvlgOUsxDYznFPWS1PpLg=";
    }
}