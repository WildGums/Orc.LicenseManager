// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAccountService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Services
{
    using System.Collections.Generic;

    public interface IAccountService
    {
        #region Methods
        void CreateUserWithRoles(string userName, string password, List<string> userRoles);
        bool RoleExists(string rolestr);
        void CreateRole(string role);
        #endregion
    }
}