// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkValidatedEventArgs.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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