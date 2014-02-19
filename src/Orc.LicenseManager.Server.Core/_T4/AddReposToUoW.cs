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
        private ILicenseRepository _LicenseRepository;

        public ILicenseRepository Licenses
        {
            get 
			{ 
				return _LicenseRepository ?? (_LicenseRepository = GetRepository<ILicenseRepository>());
		    }
        }
	}
	public partial interface IUoW
	{
            ILicenseRepository Licenses { get; }
	}
}


