namespace Orc.LicenseManager.Server.Services
{
    public interface ILicenseGenerationService
    {
        void GenerateLicenseValue(LicensePoco license);
        void GenerateKeysForProduct(Product product);
        void GeneratePassPhraseForProduct(Product product);
    }
}
