namespace Orc.LicenseManager.Server
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.IoC;
    using Services;

    public partial class  UoW
    {
        public override void SaveChanges()
        {
            UpdateCreationAndModificationDates();

            base.SaveChanges();
        }

        public override Task SaveChangesAsync()
        {
            UpdateCreationAndModificationDates();

            return base.SaveChangesAsync();
        }

        private void UpdateCreationAndModificationDates()
        {
            foreach (var ihascreatedate in DbContext.ChangeTracker.Entries<ICreator>().Where(x => x.State == EntityState.Added))
            {
                if (string.IsNullOrWhiteSpace(ihascreatedate.Entity.CreatorId))
                {
                    var membershipService = ServiceLocator.Default.ResolveType<IMembershipService>();
                    ihascreatedate.Entity.CreatorId = membershipService.GetUserId();
                }
            }
            foreach (var ihascreatedate in DbContext.ChangeTracker.Entries<ICreateDate>().Where(x => x.State == EntityState.Added))
            {
                ihascreatedate.Entity.CreationDate = DateTime.UtcNow;
            }
            foreach (var ihasmodifydate in DbContext.ChangeTracker.Entries<IModifyDate>().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added))
            {
                ihasmodifydate.Entity.ModificationDate = DateTime.UtcNow;
            }
        }
    }
}
