// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseManagerDbContext.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server
{
    using System;
    using System.Data.Entity;
    using System.Linq;
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
        #endregion
        public override int SaveChanges()
        {
            foreach (var ihascreatedate in ChangeTracker.Entries<ICreateDate>().Where(x => x.State == EntityState.Added))
            {
                ihascreatedate.Entity.CreationDate = DateTime.UtcNow; // TODO: UTC or not ?
            }
            foreach (var ihasmodifydate in ChangeTracker.Entries<IModifyDate>().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added))
            {
                ihasmodifydate.Entity.ModificationDate = DateTime.UtcNow;
            }
            return base.SaveChanges();
        }
    }
}