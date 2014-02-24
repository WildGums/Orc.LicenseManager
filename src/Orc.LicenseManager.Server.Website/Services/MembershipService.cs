// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MembershipService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website.Services
{
    using System;
    using System.Web;
    using System.Web.Security;
    using Microsoft.AspNet.Identity;
    using Server.Services;

    public class MembershipService : IMembershipService
    {
        #region IMembershipService Members
        public string GetUserId()
        {
            if (HttpContext.Current.User.Identity != null)
            {
                return HttpContext.Current.User.Identity.GetUserId();
            }
            throw new Exception("There was no membershipUser and so there can't be a userid");
        }
        #endregion
    }
}