namespace Orc.LicenseManager.Server
{
    using System;

    public interface ICreator   
    {
        string CreatorId { get; set; }
        User Creator { get; set; }
    }
}
