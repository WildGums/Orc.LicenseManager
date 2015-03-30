namespace Orc.LicenseManager.Server
{
    using System;

    public interface IModifyDate
    {
        #region Properties
        DateTime ModificationDate { get; set; }
        #endregion
    }
}