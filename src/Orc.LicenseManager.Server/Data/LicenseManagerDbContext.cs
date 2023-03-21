namespace Orc.LicenseManager.Server;

using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

public class LicenseManagerDbContext : IdentityDbContext<User>
{
    public LicenseManagerDbContext()
        : base("DefaultConnection", false)
    {
    }

    public DbSet<LicensePoco> Licenses { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public new IDbSet<Role> Roles { get; set; }
}
