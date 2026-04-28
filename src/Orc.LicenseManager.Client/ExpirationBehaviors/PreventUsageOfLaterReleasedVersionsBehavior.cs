namespace Orc.LicenseManager;

using System;
using Catel.Reflection;
using Microsoft.Extensions.Logging;
using Portable.Licensing;

public class PreventUsageOfLaterReleasedVersionsExpirationBehavior : ExpirationBehaviorBase
{
    public PreventUsageOfLaterReleasedVersionsExpirationBehavior(ILogger<PreventUsageOfLaterReleasedVersionsExpirationBehavior> logger) 
        : base(logger)
    {
    }

    protected override bool IsNormalLicenseExpired(License license, DateTime expirationDateTime, DateTime validationDateTime)
    {
        var entryAssembly = AssemblyHelper.GetRequiredEntryAssembly();
        var linkerTimestamp = entryAssembly.GetBuildDateTime();

        return (linkerTimestamp > expirationDateTime);
    }
}
