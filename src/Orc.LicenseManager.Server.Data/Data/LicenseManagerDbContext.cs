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
        public IDbSet<LicensePoco> Licenses { get; set; }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<Customer> Customers { get; set; }
        public new IDbSet<Role> Roles { get; set; }
        #endregion

        #region Methods
        public override int SaveChanges()
        {
            //foreach (var ihascreatedate in ChangeTracker.Entries<ICreator>().Where(x => x.State == EntityState.Added))
            //{
            //    var membershipService = ServiceLocator.Default.ResolveType<IMembershipService>();
            //    ihascreatedate.Entity.CreatorId = membershipService.GetUserId();
            //}
            //foreach (var ihascreatedate in ChangeTracker.Entries<ICreateDate>().Where(x => x.State == EntityState.Added))
            //{
            //    ihascreatedate.Entity.CreationDate = DateTime.UtcNow;
            //}
            //foreach (var ihasmodifydate in ChangeTracker.Entries<IModifyDate>().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added))
            //{
            //    ihasmodifydate.Entity.ModificationDate = DateTime.UtcNow;
            //}
            return base.SaveChanges();
        }
        #endregion
    }
}