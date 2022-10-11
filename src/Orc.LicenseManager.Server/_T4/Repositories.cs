using Orc.EntityFramework.Repositories;


namespace Orc.LicenseManager.Server.Repositories
{
    public partial class LicensePocoRepository : EntityRepositoryBase<LicensePoco, int>, ILicensePocoRepository
	{
		 public LicensePocoRepository(LicenseManagerDbContext dbContext) 
		 : base(dbContext)
         {
         }
	} 

    public partial class ProductRepository : EntityRepositoryBase<Product, int>, IProductRepository
	{
		 public ProductRepository(LicenseManagerDbContext dbContext) 
		 : base(dbContext)
         {
         }
	} 

    public partial class CustomerRepository : EntityRepositoryBase<Customer, int>, ICustomerRepository
	{
		 public CustomerRepository(LicenseManagerDbContext dbContext) 
		 : base(dbContext)
         {
         }
	} 

    public partial class RoleRepository : EntityRepositoryBase<Role, string>, IRoleRepository
	{
		 public RoleRepository(LicenseManagerDbContext dbContext) 
		 : base(dbContext)
         {
         }
	} 

    public partial class UserRepository : EntityRepositoryBase<User, string>, IUserRepository
	{
		 public UserRepository(LicenseManagerDbContext dbContext) 
		 : base(dbContext)
         {
         }
	} 

}

