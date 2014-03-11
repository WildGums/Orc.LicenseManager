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
        licenseService.Initialize("MIIBKjCB4wYHKoZIzj0CATCB1wIBATAsBgcqhkjOPQEBAiEA/////wAAAAEAAAAAAAAAAAAAAAD///////////////8wWwQg/////wAAAAEAAAAAAAAAAAAAAAD///////////////wEIFrGNdiqOpPns+u9VXaYhrxlHQawzFOw9jvOPD4n0mBLAxUAxJ02CIbnBJNqZnjhE50mt4GffpAEIQNrF9Hy4SxCR/i85uVjpEDydwN9gS3rM6D0oTlF2JjClgIhAP////8AAAAA//////////+85vqtpxeehPO5ysL8YyVRAgEBA0IABMfkNpgihjjgixHx3wdpazaj3gOgAWyTOqTTRSmUzQ1bc6zeX0HfHvmvU6UznL2+RqIAF1SHMtJliOjUsZoEQvU=");
    }
}