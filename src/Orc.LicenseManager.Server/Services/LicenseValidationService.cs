namespace Orc.LicenseManager.Server.Services
{
    using System.Threading.Tasks;
    using Catel;
    using Models;

    public class LicenseValidationService : ILicenseValidationService
    {
        public LicenseValidationService()
        {
            
        }

        public async Task<LicenseValidationResult> ValidateLicenseAsync(string license)
        {
            //Argument.IsNotNullOrWhitespace(() => license);

            // TODO: Verify license

            return new LicenseValidationResult(true, string.Empty);
        }
    }
}
