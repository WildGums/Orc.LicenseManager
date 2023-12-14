namespace Orc.LicenseManager;

using System;
using Catel.Reflection;
using Portable.Licensing;

public class PreventUsageOfLaterReleasedVersionsExpirationBehavior : ExpirationBehaviorBase
{
    protected override bool IsNormalLicenseExpired(License license, DateTime expirationDateTime, DateTime validationDateTime)
    {
        var entryAssembly = AssemblyHelper.GetRequiredEntryAssembly();
        var linkerTimestamp = entryAssembly.GetBuildDateTime();

        return (linkerTimestamp > expirationDateTime);
    }
}