namespace Orc.LicenseManager
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Catel;
    using Catel.Logging;
    using FileSystem;
    using Portable.Licensing;

    /// <summary>
    /// Service to validate, store and remove licenses for software products.
    /// </summary>
    public class LicenseService : ILicenseService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ILicenseLocationService _licenseLocationService;
        private readonly IFileService _fileService;

        private Tuple<License, LicenseMode> _currentLicense;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseService" /> class.
        /// </summary>
        /// <param name="licenseLocationService">The application identifier service.</param>
        /// <param name="fileService">The file service.</param>
        public LicenseService(ILicenseLocationService licenseLocationService, IFileService fileService)
        {
            Argument.IsNotNull(() => licenseLocationService);
            Argument.IsNotNull(() => fileService);

            _licenseLocationService = licenseLocationService;
            _fileService = fileService;
        }

        public License CurrentLicense
        {
            get => _currentLicense?.Item1;
        }

        /// <summary>
        /// Raised when the current license changes.
        /// </summary>
        public event EventHandler<EventArgs> CurrentLicenseChanged;

        /// <summary>
        /// Saves the license.
        /// </summary>
        /// <param name="license">The license key that will be saved to <c>Catel.IO.Path.GetApplicationDataDirectory</c> .</param>
        /// <param name="licenseMode"></param>
        /// <returns>Returns only true if the license is valid.</returns>
        /// <exception cref="ArgumentException">The <paramref name="license" /> is <c>null</c> or whitespace.</exception>
        public void SaveLicense(string license, LicenseMode licenseMode = LicenseMode.CurrentUser)
        {
            Argument.IsNotNullOrWhitespace("license", license);

            try
            {
                var licenseObject = License.Load(license);

                var xmlFilePath = _licenseLocationService.GetLicenseLocation(licenseMode);

                using (var xmlWriter = XmlWriter.Create(xmlFilePath))
                {
                    licenseObject.Save(xmlWriter);

                    xmlWriter.Flush();
                    xmlWriter.Close();
                }

                Log.Info("License saved");

                if (_currentLicense is null || _currentLicense.Item2 == licenseMode)
                {
                    LoadLicense(licenseMode);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save license");
                throw;
            }
        }

        /// <summary>
        /// Removes the license if exists.
        /// </summary>
        /// <param name="licenseMode"></param>
        public void RemoveLicense(LicenseMode licenseMode = LicenseMode.CurrentUser)
        {
            var xmlFilePath = _licenseLocationService.GetLicenseLocation(licenseMode);

            try
            {
                _fileService.Delete(xmlFilePath);

                Log.Info($"The '{licenseMode}' license has been removed");

                if (_currentLicense?.Item2 == licenseMode)
                {
                    SetCurrentLicense(null, licenseMode);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete the license @ '{xmlFilePath}'");
            }
        }

        /// <summary>
        /// Check if the license exists.
        /// </summary>
        /// <returns>returns <c>true</c> if exists else <c>false</c></returns>
        public bool LicenseExists(LicenseMode licenseMode = LicenseMode.CurrentUser)
        {
            var xmlFilePath = _licenseLocationService.GetLicenseLocation(licenseMode);

            try
            {
                if (!string.IsNullOrWhiteSpace(xmlFilePath) && _fileService.Exists(xmlFilePath))
                {
                    Log.Debug("License exists");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, $"Failed to check whether the license exists @ '{xmlFilePath}'");
            }

            Log.Debug("License does not exist");

            return false;
        }

        /// <summary>
        /// Loads the license.
        /// </summary>
        /// <returns>The license from <c>Catel.IO.Path.GetApplicationDataDirectory</c> unless it failed to load then it returns an empty string</returns>
        public string LoadLicense(LicenseMode licenseMode = LicenseMode.CurrentUser)
        {
            try
            {
                var licenseString = _licenseLocationService.LoadLicense(licenseMode);
                if (!string.IsNullOrWhiteSpace(licenseString))
                {
                    var licenseObject = License.Load(licenseString);

                    SetCurrentLicense(licenseObject, licenseMode);

                    //Log.Debug("License loaded: {0}", licenseObject.ToString());

                    return licenseObject.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load the license");
            }

            Log.Debug("Failed to load the license, returning empty string");

            SetCurrentLicense(null, licenseMode);

            return string.Empty;
        }

        /// <summary>
        /// Loads the XML out of license.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <returns>A List of with the xml names and values</returns>
        public List<XmlDataModel> LoadXmlFromLicense(string license)
        {
            var xmlDataList = new List<XmlDataModel>();

            if (string.IsNullOrWhiteSpace(license))
            {
                return xmlDataList;
            }

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(license);
                var xmlRoot = xmlDoc.DocumentElement;
                var xmlNodes = xmlRoot.ChildNodes;

                foreach (XmlNode node in xmlNodes)
                {
                    if (string.Equals(node.Name, "Customer"))
                    {
                        var customerInfo = $"{node.ChildNodes[0].InnerText} ({node.ChildNodes[1].InnerText})";
                        xmlDataList.Add(new XmlDataModel("Licensed to", customerInfo));
                    }
                    else if (string.Equals(node.Name, "ProductFeatures"))
                    {
                        foreach (XmlNode featureNode in node.ChildNodes)
                        {
                            xmlDataList.Add(new XmlDataModel
                            {
                                Name = featureNode.Attributes[0].Value,
                                Value = featureNode.InnerText
                            });
                        }
                    }
                    else
                    {
                        xmlDataList.Add(new XmlDataModel
                        {
                            Name = node.Name,
                            Value = node.InnerText
                        });
                    }
                }

                Log.Debug("Returning xml successful");
            }
            catch (Exception ex)
            {
                Log.Debug(ex);
                return new List<XmlDataModel>();
            }

            return xmlDataList;
        }

        private void SetCurrentLicense(License license, LicenseMode licenseMode)
        {
            var currentLicense = _currentLicense?.Item1;
            if (ReferenceEquals(currentLicense, license))
            {
                return;
            }

            if (currentLicense?.Id == license?.Id)
            {
                return;
            }

            _currentLicense = license is not null ? new Tuple<License, LicenseMode>(license, licenseMode) : null;

            CurrentLicenseChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
