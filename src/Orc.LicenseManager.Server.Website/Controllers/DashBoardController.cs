using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Orc.LicenseManager.Server.Website.Controllers
{
    using System.Collections;
    using Catel.Data;
    using Newtonsoft.Json;

    public class DashboardController : ApiController
    {
        [ActionName("GetLast5Products")]
        public List<Product> GetLast5Products()
        {

            using (var uow = new UoW())
            {
                var products = uow.Products.GetQuery().Include(x=>x.Licenses).OrderByDescending(x => x.CreationDate).Take(5);
                //return JsonConvert.SerializeObject(products);
                return products.ToList();
            }
        }
        [ActionName("GetLast5Customers")]
        public void GetLast5Customers()
        {
            
        }
        [ActionName("GetLast5Licenses")]
        public void GetLast5Licenses()
        {
            
        }
    }
}
