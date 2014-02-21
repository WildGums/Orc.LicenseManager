using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orc.LicenseManager.Server.Data
{
    using System.Data.Entity;

    public class RecreateContext : DropCreateDatabaseAlways<LicenseManagerDbContext>
    {
        protected override void Seed(LicenseManagerDbContext context)
        {

            base.Seed(context);
        }
    }
}
