namespace Orc.LicenseManager;

public class ApplicationIdService : IApplicationIdService
{
    public string? ApplicationId { get; set; }

    public string? CompanyName { get; set; }

    public string? ProductName { get; set; }
}