// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Role.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server
{
    using Microsoft.AspNet.Identity.EntityFramework;

    public class Role : IdentityRole
    {
        #region Constructors
        public Role()
        {
        }

        public Role(string rolename)
        {
            Name = rolename;
        }
        #endregion
    }
}