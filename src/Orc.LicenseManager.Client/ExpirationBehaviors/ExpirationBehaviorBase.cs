namespace Orc.LicenseManager;

using System;
using Catel.Logging;
using Portable.Licensing;

public abstract class ExpirationBehaviorBase : IExpirationBehavior
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    public virtual bool IsExpired(License license, DateTime expirationDateTime, DateTime validationDateTime)
    {
        ArgumentNullException.ThrowIfNull(license);

        if (license.Type != LicenseType.Trial)
        {
            return IsNormalLicenseExpired(license, expirationDateTime, validationDateTime);
        }

        Log.Debug("License is trial, checking for absolute expiration date time (trials always prevent usage after expiration date)");

        return validationDateTime > expirationDateTime;

    }

    protected abstract bool IsNormalLicenseExpired(License license, DateTime expirationDateTime, DateTime validationDateTime);
}
