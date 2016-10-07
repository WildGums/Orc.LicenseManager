// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkValidatedEventArgs.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using Models;

    public class NetworkValidatedEventArgs : EventArgs
    {
        public NetworkValidatedEventArgs(NetworkValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }

        public NetworkValidationResult ValidationResult { get; private set; }
    }
}