// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreventUsageOfAnyVersionExpirationBehavior.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;

    public class PreventUsageOfAnyVersionExpirationBehavior : ExpirationBehaviorBase
    {
        public override bool IsExpired(DateTime expirationDateTime)
        {
            // Always check date of the user (not UTC date)
            return (DateTime.Now > expirationDateTime);
        }
    }
}