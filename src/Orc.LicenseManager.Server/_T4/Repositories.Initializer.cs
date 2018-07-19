using System.Data.Entity;
using Orc.EntityFramework.Repositories;
using Orc.EntityFramework;
using Catel.IoC;
using Orc.LicenseManager.Server.Repositories;
using Orc.LicenseManager.Server;

namespace Orc.LicenseManager.Server
{
	internal static partial class RepositoryInitializer
	{
		internal static void RegisterRepositories()
		{
            ServiceLocator.Default.RegisterType<ILicensePocoRepository, LicensePocoRepository>();
	            ServiceLocator.Default.RegisterType<IProductRepository, ProductRepository>();
	            ServiceLocator.Default.RegisterType<ICustomerRepository, CustomerRepository>();
	            ServiceLocator.Default.RegisterType<IRoleRepository, RoleRepository>();
	            ServiceLocator.Default.RegisterType<IUserRepository, UserRepository>();
			}
	}
}
