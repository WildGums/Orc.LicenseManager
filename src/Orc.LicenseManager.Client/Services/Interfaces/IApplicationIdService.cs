namespace Orc.LicenseManager;

public interface IApplicationIdService
{
    string? ApplicationId { get; set; }

    string? CompanyName { get; set; }

    string? ProductName { get; set; }
}