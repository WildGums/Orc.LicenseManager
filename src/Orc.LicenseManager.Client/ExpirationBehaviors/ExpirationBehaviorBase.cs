namespace Orc.LicenseManager;

using System;
using Catel.Logging;
using Microsoft.Extensions.Logging;
using Portable.Licensing;

public abstract class ExpirationBehaviorBase : IExpirationBehavior
{
    private readonly ILogger _logger;

    protected ExpirationBehaviorBase(ILogger logger)
    {
        _logger = logger;
    }

    public virtual bool IsExpired(License license, DateTime expirationDateTime, DateTime validationDateTime)
    {
        ArgumentNullException.ThrowIfNull(license);

        if (license.Type != LicenseType.Trial)
        {
            return IsNormalLicenseExpired(license, expirationDateTime, validationDateTime);
        }

        _logger.LogDebug("License is trial, checking for absolute expiration date time (trials always prevent usage after expiration date)");

        return validationDateTime > expirationDateTime;

    }

    protected abstract bool IsNormalLicenseExpired(License license, DateTime expirationDateTime, DateTime validationDateTime);
}
