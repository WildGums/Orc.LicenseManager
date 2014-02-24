namespace Orc.LicenseManager.Server
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using Catel.IoC;
    using Services;

    public partial class  UoW
    {
        public override void SaveChanges(SaveOptions saveOptions = SaveOptions.None | SaveOptions.AcceptAllChangesAfterSave | SaveOptions.DetectChangesBeforeSave)
        {
            foreach (var ihascreatedate in DbContext.ChangeTracker.Entries<ICreator>().Where(x => x.State == EntityState.Added))
            {
                var membershipService = ServiceLocator.Default.ResolveType<IMembershipService>();
                ihascreatedate.Entity.CreatorId = membershipService.GetUserId();
            }
            foreach (var ihascreatedate in DbContext.ChangeTracker.Entries<ICreateDate>().Where(x => x.State == EntityState.Added))
            {
                ihascreatedate.Entity.CreationDate = DateTime.UtcNow;
            }
            foreach (var ihasmodifydate in DbContext.ChangeTracker.Entries<IModifyDate>().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added))
            {
                ihasmodifydate.Entity.ModificationDate = DateTime.UtcNow;
            }
            base.SaveChanges(saveOptions);
        }
    }
}
