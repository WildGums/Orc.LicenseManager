// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseInfoService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using Models;

    public class LicenseInfoService : ILicenseInfoService
    {
        public LicenseInfo GetLicenseInfo()
        {
            var licenseInfo = new LicenseInfo();

            licenseInfo.Title = "Catel";
            licenseInfo.ImageUri = "/Orc.LicenseManager.Client.WPF.Example;component/Resources/Images/logo_with_text.png";
            licenseInfo.Text = "Catel is a company made in 2010 and is  dolor sit amet, consectetur adipiscing elit. Etiam nec sem sit amet felis blandit semper. Morbi tempus ligula urna, feugiat rhoncus dolor elementum non.";
            licenseInfo.InfoUrl = "http://www.catelproject.com/";
            licenseInfo.PurchaseUrl = "http://www.catelproject.com/product/buy/642";

            return licenseInfo;
        }
    }
}