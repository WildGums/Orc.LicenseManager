// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreventUsageOfAnyVersionExpirationBehavior.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;
    using Catel.Reflection;
    using Portable.Licensing;

    public class PreventUsageOfLaterReleasedVersionsExpirationBehavior : ExpirationBehaviorBase
    {
        protected override bool IsNormalLicenseExpired(License license, DateTime expirationDateTime, DateTime validationDateTime)
        {
            var entryAssembly = AssemblyHelper.GetEntryAssembly();
            var linkerTimestamp = entryAssembly.GetBuildDateTime();

            return (linkerTimestamp > expirationDateTime);
        }
    }
}