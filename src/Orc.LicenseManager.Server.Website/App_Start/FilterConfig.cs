// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterConfig.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LicenseManager.Server.Website
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