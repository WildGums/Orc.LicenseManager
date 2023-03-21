namespace Orc.LicenseManager;

using System;
using Portable.Licensing;

public class PreventUsageOfAnyVersionExpirationBehavior : ExpirationBehaviorBase
{
    protected override bool IsNormalLicenseExpired(License license, DateTime expirationDateTime, DateTime validationDateTime)
    {
        // Always check date of the user (not UTC date)
        return (validationDateTime > expirationDateTime);
    }
}