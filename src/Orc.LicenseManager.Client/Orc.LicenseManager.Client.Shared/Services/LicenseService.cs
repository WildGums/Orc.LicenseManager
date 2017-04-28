// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using Catel;
    using Catel.Logging;
    using FileSystem;
    using Models;
    using Portable.Licensing;

    /// <summary>
    /// Service to validate, store and remove licenses for software products.
    /// </summary>
    public class LicenseService : ILicenseService
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly ILicenseLocationService _licenseLocationService;
        private readonly IFileService _fileService;
        #endregion

        #region Constructors
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
        #endregion

        #region Properties
        public License CurrentLicense { get; private set; }
        #endregion

        #region ILicenseService Members
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

                Log.Info("The '{0}' license has been removed", licenseMode);
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
                if (_fileService.Exists(xmlFilePath))
                {
                    Log.Debug("License exists");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, $"Failed to check whether the license exists @ 'xmlFilePath'");
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
                var licenseObject = License.Load(licenseString);

                CurrentLicense = licenseObject;

                //Log.Debug("License loaded: {0}", licenseObject.ToString());

                return licenseObject.ToString();
            }
            catch (Exception ex)
            {
                Log.Debug(ex, "Failed to load the license, returning empty string");
                return string.Empty;
            }
        }

        /// <summary>
        /// Loads the XML out of license.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <returns>A List of with the xml names and values</returns>
        public List<XmlDataModel> LoadXmlFromLicense(string license)
        {
            var xmlDataList = new List<XmlDataModel>();

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
                        foreach (XmlNode featrureNode in node.ChildNodes)
                        {
                            xmlDataList.Add(new XmlDataModel
                            {
                                Name = featrureNode.Attributes[0].Value,
                                Value = featrureNode.InnerText
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
                return null;
            }

            return xmlDataList;
        }
        #endregion
    }
}