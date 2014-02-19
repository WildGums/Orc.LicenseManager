// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseManagerDbContext.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class LicenseManagerDbContext : IdentityDbContext<User>
    {
        #region Constructors
        public LicenseManagerDbContext()
            : base("DefaultConnection")
        {
        }
        #endregion

       public DbSet<License> Licenses { get; set; }
    }
}