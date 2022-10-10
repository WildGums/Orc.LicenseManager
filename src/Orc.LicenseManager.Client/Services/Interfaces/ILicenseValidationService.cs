namespace Orc.LicenseManager
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml;
    using Catel.Data;
    
    public interface ILicenseValidationService
    {
        /// <summary>
        /// Validates the license.
        /// </summary>
        /// <param name="license">The license key the user has given to be validated.</param>
        /// <returns>The validation context containing all the validation results.</returns>
        /// <exception cref="System.Exception">Please use the Initialize method first</exception>
        Task<IValidationContext> ValidateLicenseAsync(string license);

        /// <summary>
        /// Validates the license on the server.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <param name="serverUrl">The server URL.</param>
        /// <param name="assembly">The assembly to get the information from. If <c>null</c>, the entry assembly will be used.</param>
        /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
        Task<LicenseValidationResult> ValidateLicenseOnServerAsync(string license, string serverUrl, Assembly assembly = null);

        /// <summary>
        /// Validates the XML
        /// </summary>
        /// <param name="license">The license.</param>
        /// <returns>The validation context containing all the validation results.</returns>
        /// <exception cref="ArgumentException">The <paramref name="license" /> is <c>null</c> or whitespace.</exception>
        /// <exception cref="XmlException">The license text is not valid XML.</exception>
        /// <exception cref="Exception">The root element is not License.</exception>
        /// <exception cref="Exception">There were no inner nodes found.</exception>
        Task<IValidationContext> ValidateXmlAsync(string license);
    }
}
