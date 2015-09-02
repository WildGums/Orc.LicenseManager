using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orc.LicenseManager.Server.Website.Controllers
{
    using System.Reflection.Emit;
    using Microsoft.AspNet.Identity;
    [Authorize(Roles = "Admin")]
    public abstract class BaseController : Controller
    {
        public string UserIP {
            get { return Request.ServerVariables["REMOTE_ADDR"]; }
        }
        public string UserID
        {
            get { return User.Identity.GetUserId(); }
        }
	}
}