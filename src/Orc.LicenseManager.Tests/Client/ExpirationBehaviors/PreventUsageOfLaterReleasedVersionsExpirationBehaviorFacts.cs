﻿namespace Orc.LicenseManager.Tests.Client.ExpirationBehaviors;

using System;
using System.Globalization;
using NUnit.Framework;
using Portable.Licensing;

[TestFixture]
public class PreventUsageOfLaterReleasedVersionsExpirationBehaviorFacts
{
    // Note: hard to test because it checks the build timestamp
    //[TestCase(LicenseType.Standard, "2014-11-29", "2014-11-28", false)]
    //[TestCase(LicenseType.Standard, "2014-11-29", "2014-11-29", false)]
    //[TestCase(LicenseType.Standard, "2014-11-29", "2014-11-30", false)]
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

        Assert.That(expirationBehavior.IsExpired(license, expirationDate, currentDate), Is.EqualTo(expectedValue));
    }
}
