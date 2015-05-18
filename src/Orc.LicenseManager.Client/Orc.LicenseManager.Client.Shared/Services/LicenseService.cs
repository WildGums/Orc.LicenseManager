// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using Catel;
    using Catel.Logging;
    using Models;
    using Portable.Licensing;

    /// <summary>
    /// Service to validate, store and remove licenses for software products.
    /// </summary>
    public class LicenseService : ILicenseService
    {
        private readonly IApplicationIdService _applicationIdService;

        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseService" /> class.
        /// </summary>
        /// <param name="applicationIdService">The application identifier service.</param>
        public LicenseService(IApplicationIdService applicationIdService)
        {
            Argument.IsNotNull(() => applicationIdService);

            _applicationIdService = applicationIdService;
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
                string xmlFilePath = GetLicenseInfoPath(licenseMode);
                var licenseObject = License.Load(license);

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
            string xmlFilePath = GetLicenseInfoPath(licenseMode);

            if (File.Exists(xmlFilePath))
            {
                File.Delete(xmlFilePath);
            }

            Log.Info("The '{0}' License has been removed", licenseMode);
        }

        /// <summary>
        /// Check if the license exists.
        /// </summary>
        /// <returns>returns <c>true</c> if exists else <c>false</c></returns>
        public bool LicenseExists(LicenseMode licenseMode = LicenseMode.CurrentUser)
        {
            string xmlFilePath = GetLicenseInfoPath(licenseMode);
            if (File.Exists(xmlFilePath))
            {
                Log.Debug("License exists");
                return true;
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
                var xmlFilePath = GetLicenseInfoPath(licenseMode);

                using (var xmlReader = XmlReader.Create(xmlFilePath))
                {
                    var licenseObject = License.Load(xmlReader);

                    CurrentLicense = licenseObject;

                    Log.Debug("License loaded: {0}", licenseObject.ToString());

                    return licenseObject.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Failed to load the license, returning empty string");
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
                        var customerInfo = string.Format("{0} ({1})", node.ChildNodes[0].InnerText, node.ChildNodes[1].InnerText);
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

        #region Methods
        private string GetLicenseInfoPath(LicenseMode licenseMode)
        {
            if (licenseMode == LicenseMode.CurrentUser)
            {
                return Path.Combine(Catel.IO.Path.GetApplicationDataDirectory(), "LicenseInfo.xml");
            }

            return Path.Combine(Catel.IO.Path.GetApplicationDataDirectoryForAllUsers(), "LicenseInfo.xml");
        }
        #endregion
    }
}