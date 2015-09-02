// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationIdService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    public class ApplicationIdService : IApplicationIdService
    {
        public string ApplicationId { get; set; }

        public string CompanyName { get; set; }

        public string ProductName { get; set; }
    }
}