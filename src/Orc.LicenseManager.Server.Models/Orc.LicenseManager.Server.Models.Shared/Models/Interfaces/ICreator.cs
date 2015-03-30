namespace Orc.LicenseManager.Server
{
    using System;

    public interface ICreator   
    {
        #region Properties
        string CreatorId { get; set; }
        User Creator { get; set; }
        #endregion
    }
}