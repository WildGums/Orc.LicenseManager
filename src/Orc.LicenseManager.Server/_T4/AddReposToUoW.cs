using System.Data.Entity;
using Orc.EntityFramework.Repositories;
using Orc.EntityFramework;
using Catel.IoC;
using Orc.LicenseManager.Server.Repositories;
using Orc.LicenseManager.Server;


namespace Orc.LicenseManager.Server
{

	public partial class UoW : UnitOfWork<LicenseManagerDbContext>, IUoW
	{
        private ILicensePocoRepository _LicensePocoRepository;

        public ILicensePocoRepository Licenses
        {
            get 
			{ 
				return _LicensePocoRepository ??= GetRepository<ILicensePocoRepository>();
		    }
        }
        private IProductRepository _ProductRepository;

        public IProductRepository Products
        {
            get 
			{ 
				return _ProductRepository ??= GetRepository<IProductRepository>();
		    }
        }
        private ICustomerRepository _CustomerRepository;

        public ICustomerRepository Customers
        {
            get 
			{ 
				return _CustomerRepository ??= GetRepository<ICustomerRepository>();
		    }
        }
        private IRoleRepository _RoleRepository;

        public IRoleRepository Roles
        {
            get 
			{ 
				return _RoleRepository ??= GetRepository<IRoleRepository>();
		    }
        }
        private IUserRepository _UserRepository;

        public IUserRepository Users
        {
            get 
			{ 
				return _UserRepository ??= GetRepository<IUserRepository>();
		    }
        }
	}
	public partial interface IUoW
	{
            ILicensePocoRepository Licenses { get; }
            IProductRepository Products { get; }
            ICustomerRepository Customers { get; }
            IRoleRepository Roles { get; }
            IUserRepository Users { get; }
	}
}


