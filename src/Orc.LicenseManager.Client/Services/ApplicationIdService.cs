// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationIdService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    public class ApplicationIdService : IApplicationIdService
    {
        public string ApplicationId { get; set; }

        public string CompanyName { get; set; }

        public string ProductName { get; set; }
    }
}
