// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseServiceFacts.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Test.Client.Services
{
    using System;
    using Catel.IoC;
    using Catel.Test;
    using LicenseManager.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}