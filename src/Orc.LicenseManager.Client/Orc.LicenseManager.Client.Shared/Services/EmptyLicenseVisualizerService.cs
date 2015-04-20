// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyRequestLicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System.Threading.Tasks;

    public class EmptyLicenseVisualizerService : ILicenseVisualizerService
    {
        public async Task ShowLicense()
        {
        }
    }
}