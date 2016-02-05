// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterConfig.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website
{
    using System.Web.Mvc;

    public class FilterConfig
    {
        #region Methods
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        #endregion
    }
}