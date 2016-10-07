// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationIdService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    public interface IApplicationIdService
    {
        string ApplicationId { get; set; }

        string CompanyName { get; set; }

        string ProductName { get; set; }
    }
}