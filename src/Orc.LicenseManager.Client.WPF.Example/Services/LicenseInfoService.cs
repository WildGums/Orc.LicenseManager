namespace Orc.LicenseManager.Services
{
    public class LicenseInfoService : ILicenseInfoService
    {
        public LicenseInfo GetLicenseInfo()
        {
            var licenseInfo = new LicenseInfo(
                "Catel",
                "/Orc.LicenseManager.Client.WPF.Example.NET;component/Resources/Images/logo_with_text.png",
                "Catel is a company made in 2010 and is  dolor sit amet, consectetur adipiscing elit. Etiam nec sem sit amet felis blandit semper. Morbi tempus ligula urna, feugiat rhoncus dolor elementum non.",
                "http://www.catelproject.com/",
                "http://www.catelproject.com/product/buy/642");

            return licenseInfo;
        }
    }
}
