namespace Orc.LicenseManager.Services
{
    public class LicenseInfoService : ILicenseInfoService
    {
        public LicenseInfo GetLicenseInfo()
        {
            var licenseInfo = new LicenseInfo();

            licenseInfo.Title = "Catel";
            licenseInfo.ImageUri = "/Orc.LicenseManager.Client.WPF.Example.NET;component/Resources/Images/logo_with_text.png";
            licenseInfo.Text = "Catel is a company made in 2010 and is  dolor sit amet, consectetur adipiscing elit. Etiam nec sem sit amet felis blandit semper. Morbi tempus ligula urna, feugiat rhoncus dolor elementum non.";
            licenseInfo.InfoUrl = "http://www.catelproject.com/";
            licenseInfo.PurchaseUrl = "http://www.catelproject.com/product/buy/642";

            return licenseInfo;
        }
    }
}
