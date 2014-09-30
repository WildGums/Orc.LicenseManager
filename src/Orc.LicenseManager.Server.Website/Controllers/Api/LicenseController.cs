// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckLicenseController.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server.Website.Controllers.Api
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Catel;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Server.Services;

    public class LicenseController : ApiController
    {
        private readonly ILicenseValidationService _licenseValidationService;

        public LicenseController(ILicenseValidationService licenseValidationService)
        {
            Argument.IsNotNull(() => licenseValidationService);

            _licenseValidationService = licenseValidationService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Validate([FromBody]string license)
        {
            Argument.IsNotNull(() => license);

            var licenseValidationResult = await _licenseValidationService.ValidateLicense(license);

            return new HttpResponseMessage
            {
                Content = new JsonContent(licenseValidationResult)
            };
        }
    }
}