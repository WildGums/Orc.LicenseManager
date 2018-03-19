// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublicApiFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Tests
{
    using ApiApprover;
    using NUnit.Framework;
    using Services;

    [TestFixture]
    public class PublicApiFacts
    {
        [Test]
        public void Orc_LicenseManager_Client_HasNoBreakingChanges()
        {
            var assembly = typeof(ApplicationIdService).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }

        [Test]
        public void Orc_LicenseManager_Client_Wpf_HasNoBreakingChanges()
        {
            var assembly = typeof(DialogLicenseVisualizerService).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }
    }
}