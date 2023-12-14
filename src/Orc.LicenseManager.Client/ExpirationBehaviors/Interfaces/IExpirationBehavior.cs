namespace Orc.LicenseManager;

using System;
using Portable.Licensing;

public interface IExpirationBehavior
{
    bool IsExpired(License license, DateTime expirationDateTime, DateTime validationDateTime);
}