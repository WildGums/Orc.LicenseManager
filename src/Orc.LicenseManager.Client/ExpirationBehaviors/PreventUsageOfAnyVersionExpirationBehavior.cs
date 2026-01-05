namespace Orc.LicenseManager;

using System;
using Microsoft.Extensions.Logging;
using Portable.Licensing;

public class PreventUsageOfAnyVersionExpirationBehavior : ExpirationBehaviorBase
{
    public PreventUsageOfAnyVersionExpirationBehavior(ILogger<PreventUsageOfAnyVersionExpirationBehavior> logger) 
        : base(logger)
    {
    }

    protected override bool IsNormalLicenseExpired(License license, DateTime expirationDateTime, DateTime validationDateTime)
    {
        // Always check date of the user (not UTC date)
        return (validationDateTime > expirationDateTime);
    }
}
