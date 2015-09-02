// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationIdService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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