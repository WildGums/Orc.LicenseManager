// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseServiceFacts.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Test.Client.Services
{
    using System;
    using System.IO;
    using System.Xml;
    using Catel.IoC;
    using Catel.Test;
    using LicenseManager.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Portable.Licensing;

    public class LicenseServiceFacts
    {
        [TestClass]
        public class TheInitializeMethod
        {
            [TestMethod]
            public void ThrowsArgumentExceptionForNullApplicationId()
            {
                var typeFactory = TypeFactory.Default;
                var service = typeFactory.CreateInstance<LicenseService>();

                ExceptionTester.CallMethodAndExpectException<ArgumentException>(() => service.Initialize(null));
            }

            [TestMethod]
            public void ThrowsArgumentExceptionForWhitespaceApplicationId()
            {
                var typeFactory = TypeFactory.Default;
                var service = typeFactory.CreateInstance<LicenseService>();

                ExceptionTester.CallMethodAndExpectException<ArgumentException>(() => service.Initialize(" "));
            }
        }

        [TestClass]
        public class TheShowSingleLicenseDialog
        {
#if DEBUG
            [TestMethod]
#endif
            public void ShowsDialog()
            {
                var typeFactory = TypeFactory.Default;
                var service = typeFactory.CreateInstance<LicenseService>();

                service.ShowSingleLicenseDialog();
            }
        }

        [TestClass]
        public class LicenseIO
        {
            [TestMethod]
            public void CreateFakeLicense()
            {
                var location = Catel.IO.Path.GetApplicationDataDirectory() + "\\LicenseInfo.xml";
                bool filealreadyexisted = false;
                const string licensetext = "<License><Id>4fe5df0d-90f2-485f-a638-337bca2e28dc</Id><Type>Standard</Type><Quantity>5</Quantity><Signature>MEYCIQCVkNN/rQeeV02k/tbCuRkv+E0e34mmu8TwssHnqsRoiAIhALpuXAxm5NHrQIE/51FGBgeOqb2y/M95YBTKafn0nUmW</Signature></License>";
                if (File.Exists(location))
                {
                    filealreadyexisted = true;
                    File.Move(location, location + "_bak");
                }
                var typeFactory = TypeFactory.Default;
                var service = typeFactory.CreateInstance<LicenseService>();
                service.SaveLicense(licensetext);
           
                var xmlReader = XmlReader.Create(location);
                var licenseObject = License.Load(xmlReader);
                Assert.AreEqual(licenseObject.ToString(), licensetext);
                if (filealreadyexisted)
                {
                    File.Move(location + "_bak", location);
                }

            }
        }
    }
}