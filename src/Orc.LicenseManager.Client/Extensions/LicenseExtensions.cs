namespace Orc.LicenseManager
{
    using Catel;
    using Catel.Logging;
    using Portable.Licensing;

    public static class LicenseExtensions
    {
        public static int GetMaximumConcurrentLicenses(this License license)
        {
            Argument.IsNotNull(() => license);

            var maximumConcurrentNumbers = license.Quantity;
            if (maximumConcurrentNumbers <= 0)
            {
                maximumConcurrentNumbers = 1;
            }

            return maximumConcurrentNumbers;
        }
    }
}
