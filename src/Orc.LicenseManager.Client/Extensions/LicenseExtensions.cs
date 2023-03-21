namespace Orc.LicenseManager;

using System;
using Portable.Licensing;

public static class LicenseExtensions
{
    public static int GetMaximumConcurrentLicenses(this License license)
    {
        ArgumentNullException.ThrowIfNull(license);

        var maximumConcurrentNumbers = license.Quantity;
        if (maximumConcurrentNumbers <= 0)
        {
            maximumConcurrentNumbers = 1;
        }

        return maximumConcurrentNumbers;
    }
}