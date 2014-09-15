using System.Data.Entity;
using Catel.Data.Repositories;
using Catel.Data;
using Catel.IoC;
using Orc.LicenseManager.Server.Repositories;
using Orc.LicenseManager.Server;

namespace Orc.LicenseManager.Server.Repositories
{
	public partial interface ILicensePocoRepository : IEntityRepository<LicensePoco, int>
	{
		
	} 
	public partial interface IProductRepository : IEntityRepository<Product, int>
	{
		
	} 
	public partial interface ICustomerRepository : IEntityRepository<Customer, int>
	{
		
	} 
	public partial interface IRoleRepository : IEntityRepository<Role, string>
	{
		
	} 
	public partial interface IUserRepository : IEntityRepository<User, string>
	{
		
	} 
}

