namespace Orc.LicenseManager.Tests.Client.ExpirationBehaviors;

using System;
using System.Globalization;
using Portable.Licensing;
using Tests;
using NUnit.Framework;

[TestFixture]
public class PreventUsageOfAnyVersionExpirationBehaviorFacts
{
    [TestCase(LicenseType.Standard, "2014-11-29", "2014-11-28", false)]
    [TestCase(LicenseType.Standard, "2014-11-29", "2014-11-29", false)]
    [TestCase(LicenseType.Standard, "2014-11-29", "2014-11-30", true)]
    [TestCase(LicenseType.Trial, "2014-11-29", "2014-11-28", false)]
    [TestCase(LicenseType.Trial, "2014-11-29", "2014-11-29", false)]
    [TestCase(LicenseType.Trial, "2014-11-29", "2014-11-30", true)]
    public void ReturnsRightValue(LicenseType licenseType, string expirationDateString, string currentDateString, bool expectedValue)
    {
        var expirationDate = DateTime.ParseExact(expirationDateString, "yyyy-MM-dd", CultureInfo.CurrentCulture);
        var currentDate = DateTime.ParseExact(currentDateString, "yyyy-MM-dd", CultureInfo.CurrentCulture);

        var licenseBuilder = License.New().As(licenseType).ExpiresAt(expirationDate);
        var license = licenseBuilder.CreateAndSignWithPrivateKey(TestEnvironment.LicenseKeys.Private, TestEnvironment.LicenseKeys.PassPhrase);

        var expirationBehavior = new PreventUsageOfAnyVersionExpirationBehavior();
            
        Assert.AreEqual(expectedValue, expirationBehavior.IsExpired(license, expirationDate, currentDate));
    }
}
