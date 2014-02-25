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

        #region Properties
        public DbSet<LicensePoco> Licenses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public new IDbSet<Role> Roles { get; set; }
        #endregion
    }
}