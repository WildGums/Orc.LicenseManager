// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpirationBehaviorBase.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;

    public abstract class ExpirationBehaviorBase : IExpirationBehavior
    {
        public abstract bool IsExpired(DateTime expirationDateTime);
    }
}