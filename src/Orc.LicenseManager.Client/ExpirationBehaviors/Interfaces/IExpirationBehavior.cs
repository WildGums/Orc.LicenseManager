// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExpirationBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;
    using Portable.Licensing;

    public interface IExpirationBehavior
    {
        bool IsExpired(License license, DateTime expirationDateTime, DateTime validationDateTime);
    }
}