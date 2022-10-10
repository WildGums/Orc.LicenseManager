namespace Orc.LicenseManager
{
    using System;

    public class NetworkValidatedEventArgs : EventArgs
    {
        public NetworkValidatedEventArgs(NetworkValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }

        public NetworkValidationResult ValidationResult { get; private set; }
    }
}
