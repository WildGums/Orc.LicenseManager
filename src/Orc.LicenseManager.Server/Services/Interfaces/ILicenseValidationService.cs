namespace Orc.LicenseManager.Server.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface ILicenseValidationService
    {
        Task<LicenseValidationResult> ValidateLicenseAsync(string license);
    }
}
