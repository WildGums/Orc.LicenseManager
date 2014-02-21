using System.Data.Entity;
using Catel.Data.Repositories;
using Catel.Data;
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
				return _LicensePocoRepository ?? (_LicensePocoRepository = GetRepository<ILicensePocoRepository>());
		    }
        }
        private IProductRepository _ProductRepository;

        public IProductRepository Products
        {
            get 
			{ 
				return _ProductRepository ?? (_ProductRepository = GetRepository<IProductRepository>());
		    }
        }
	}
	public partial interface IUoW
	{
            ILicensePocoRepository Licenses { get; }
            IProductRepository Products { get; }
	}
}


