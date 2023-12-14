namespace Orc.LicenseManager.Server;

public interface ICreator   
{
    string CreatorId { get; set; }
    User Creator { get; set; }
}