namespace Orc.LicenseManager;

public interface INetworkValidationHandler
{
    void HandleNetworkValidation(object? sender, NetworkValidatedEventArgs e);
}