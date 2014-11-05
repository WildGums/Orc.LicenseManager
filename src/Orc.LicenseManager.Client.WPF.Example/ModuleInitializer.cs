using Catel.IoC;
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
        var licenseService = ServiceLocator.Default.ResolveType<ILicenseService>(); // A Valid license for this application ID aka Public Key can be found in Doc/FakeLicenseInfo.txt
        licenseService.Initialize("MIIBKjCB4wYHKoZIzj0CATCB1wIBATAsBgcqhkjOPQEBAiEA/////wAAAAEAAAAAAAAAAAAAAAD///////////////8wWwQg/////wAAAAEAAAAAAAAAAAAAAAD///////////////wEIFrGNdiqOpPns+u9VXaYhrxlHQawzFOw9jvOPD4n0mBLAxUAxJ02CIbnBJNqZnjhE50mt4GffpAEIQNrF9Hy4SxCR/i85uVjpEDydwN9gS3rM6D0oTlF2JjClgIhAP////8AAAAA//////////+85vqtpxeehPO5ysL8YyVRAgEBA0IABGO1VK0oiRQgYYynBbl+QVFk4VEAQmIqI0EkDmUcY6SP00lP2B6a6KVAtS2QY5qKld4ug0+IBm0eH7gk/E6yfOk=");

        
    }
}