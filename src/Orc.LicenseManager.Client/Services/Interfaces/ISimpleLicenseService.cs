namespace Orc.LicenseManager;

using System.Threading.Tasks;

/// <summary>
/// A very simple implementation of the license service.
/// </summary>
public interface ISimpleLicenseService
{
    /// <summary>
    /// Validates the license in a very simple manner. This method is wrapper around the <see cref="ILicenseService" />.
    /// </summary>
    /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
    /// <remarks>Note that this method might show a dialog so must be run on the UI thread.</remarks>
    Task<bool> ValidateAsync();

    /// <summary>
    /// Validates the license on the server. This method is the same as <see cref="SimpleLicenseService.ValidateAsync" /> but also checks the server if the license
    /// is valid.
    /// </summary>
    /// <param name="serverUrl">The server URL.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    Task<bool> ValidateOnServerAsync(string serverUrl);
}