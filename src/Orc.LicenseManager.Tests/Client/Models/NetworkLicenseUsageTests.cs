namespace Orc.LicenseManager.Tests.Client.Models;

using System;
using System.Threading.Tasks;
using NUnit.Framework;

public class NetworkLicenseUsageTests
{
    [TestFixture]
    public class TheParsing
    {
        [Test]
        public async Task TestParsingAsync()
        {
            var dateTime = DateTime.Now;

            var usage = new NetworkLicenseUsage("computerId", "ip", "userName", "licenseSignature", dateTime);

            Assert.That(usage.ComputerId, Is.EqualTo("computerId"));
            Assert.That(usage.Ip, Is.EqualTo("ip"));
            Assert.That(usage.LicenseSignature, Is.EqualTo("licenseSignature"));
            Assert.That(usage.UserName, Is.EqualTo("userName"));

            var networkString = await usage.ToNetworkMessageAsync();
            var usage2 = await NetworkLicenseUsage.ParseAsync(networkString);

            Assert.That(usage2.ComputerId, Is.EqualTo(usage.ComputerId));
            Assert.That(usage2.Ip, Is.EqualTo(usage.Ip));
            Assert.That(usage2.LicenseSignature, Is.EqualTo(usage.LicenseSignature));
            Assert.That(usage2.UserName, Is.EqualTo(usage.UserName));
        }
    }
}
