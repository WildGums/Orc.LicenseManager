// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpirationBehaviorBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager
{
    using System;
    using Catel;
    using Catel.Logging;
    using Portable.Licensing;

    public abstract class ExpirationBehaviorBase : IExpirationBehavior
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public virtual bool IsExpired(License license, DateTime expirationDateTime, DateTime validationDateTime)
        {
            Argument.IsNotNull(() => license);

            if (license.Type == LicenseType.Trial)
            {
                Log.Debug("License is trial, checking for absolute expiration date time (trials always prevent usage after expiration date)");

                return validationDateTime > expirationDateTime;
            }

            return IsNormalLicenseExpired(license, expirationDateTime, validationDateTime);
        }

        protected abstract bool IsNormalLicenseExpired(License license, DateTime expirationDateTime, DateTime validationDateTime);
    }
}