// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseValidationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Xml;
    using Catel;
    using Catel.Data;
    using Catel.Logging;
    using Catel.Reflection;
    using Models;
    using Newtonsoft.Json;
    using Portable.Licensing;
    using Portable.Licensing.Validation;

    public class LicenseValidationService : ILicenseValidationService
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IApplicationIdService _applicationIdService;
        private readonly IExpirationBehavior _expirationBehavior;
        private readonly IIdentificationService _identificationService;
        private readonly IMachineLicenseValidationService _machineLicenseValidationService;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseValidationService" /> class.
        /// </summary>
        /// <param name="applicationIdService">The application identifier service.</param>
        /// <param name="expirationBehavior">The expiration behavior.</param>
        /// <param name="identificationService">The identification service.</param>
        /// <param name="machineLicenseValidationService">The machine license validation service.</param>
        public LicenseValidationService(IApplicationIdService applicationIdService, IExpirationBehavior expirationBehavior,
            IIdentificationService identificationService, IMachineLicenseValidationService machineLicenseValidationService)
        {
            Argument.IsNotNull(() => applicationIdService);
            Argument.IsNotNull(() => expirationBehavior);
            Argument.IsNotNull(() => identificationService);
            Argument.IsNotNull(() => machineLicenseValidationService);

            _applicationIdService = applicationIdService;
            _expirationBehavior = expirationBehavior;
            _identificationService = identificationService;
            _machineLicenseValidationService = machineLicenseValidationService;
        }

        /// <summary>
        /// Validates the license.
        /// </summary>
        /// <param name="license">The license key the user has given to be validated.</param>
        /// <returns>The validation context containing all the validation results.</returns>
        public IValidationContext ValidateLicense(string license)
        {
            Argument.IsNotNullOrWhitespace(() => license);

            var validationContext = new ValidationContext();

            Log.Info("Validating license");

            try
            {
                var licenseObject = License.Load(license);
                var failureList = licenseObject.Validate()
                    .Signature(_applicationIdService.ApplicationId)
                    .AssertValidLicense().ToList();

                if (failureList.Count > 0)
                {
                    foreach (var failure in failureList)
                    {
                        var businessRuleValidationResult = BusinessRuleValidationResult.CreateErrorWithTag(failure.Message, failure.HowToResolve);
                        validationContext.Add(businessRuleValidationResult);
                    }
                }

                var licenseAttributes = licenseObject.AdditionalAttributes;
                if (licenseAttributes != null)
                {
                    foreach (var licenseAttribute in licenseAttributes.GetAll())
                    {
                        if (string.Equals(licenseAttribute.Key, LicenseElements.MachineId))
                        {
                            Log.Debug("Validating license using machine ID");

                            var machineLicenseValidationContext = _machineLicenseValidationService.Validate(licenseAttribute.Value);
                            validationContext.SynchronizeWithContext(machineLicenseValidationContext, true);

                            if (machineLicenseValidationContext.HasErrors)
                            {
                                Log.Error("The license can only run on machine with ID '{0}'", licenseAttribute.Value);
                            }
                        }
                    }
                }

                // Also validate the xml, very important for expiration date and version
                var xmlValidationContext = ValidateXml(license);
                validationContext.SynchronizeWithContext(xmlValidationContext, true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while loading the license");

                validationContext.Add(BusinessRuleValidationResult.CreateError("An unknown error occurred while loading the license, please contact support"));
            }
            finally
            {
                LogLicenseValidity(validationContext);
            }

            return validationContext;
        }

        /// <summary>
        /// Validates the license on the server.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <param name="serverUrl">The server URL.</param>
        /// <param name="assembly">The assembly to get the information from. If <c>null</c>, the entry assembly will be used.</param>
        /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
        public LicenseValidationResult ValidateLicenseOnServer(string license, string serverUrl, Assembly assembly = null)
        {
            Argument.IsNotNullOrWhitespace(() => license);
            Argument.IsNotNullOrWhitespace(() => serverUrl);

            if (assembly == null)
            {
                assembly = AssemblyHelper.GetEntryAssembly();
            }

            LicenseValidationResult validationResult = null;

            try
            {
                var webRequest = WebRequest.Create(serverUrl);
                webRequest.ContentType = "application/json";
                webRequest.Method = "POST";

                using (var sw = new StreamWriter(webRequest.GetRequestStream()))
                {
                    var version = "unknown version";
                    if (assembly != null)
                    {
                        try
                        {
                            version = assembly.InformationalVersion();
                            if (string.IsNullOrWhiteSpace(version))
                            {
                                version = assembly.Version();
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, "Failed to retrieve the product version");
                        }
                    }

                    var serviceLicenseValidation = new ServerLicenseValidation
                    {
                        MachineId = _identificationService.GetMachineId(),
                        ProductName = (assembly != null) ? assembly.Product() : "unknown product",
                        ProductVersion = version,
                        License = license
                    };

                    var json = JsonConvert.SerializeObject(serviceLicenseValidation);

                    sw.Write(json);
                }

                using (var httpWebResponse = webRequest.GetResponse())
                {
                    using (var responseStream = httpWebResponse.GetResponseStream())
                    {
                        var streamReader = new StreamReader(responseStream);
                        var json = streamReader.ReadToEnd();
                        validationResult = JsonConvert.DeserializeObject<LicenseValidationResult>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to validate the license on the server");
            }

            if (validationResult == null)
            {
                validationResult = new LicenseValidationResult()
                {
                    // We return success if we can't validate on the server, then it's up to the client to validate
                    IsValid = true,
                    AdditionalInfo = "Failed to check the license on the server"
                };
            }

            return validationResult;
        }

        /// <summary>
        /// Validates the XML
        /// </summary>
        /// <param name="license">The license.</param>
        /// <returns>The validation context containing all the validation results.</returns>
        /// <exception cref="ArgumentException">The <paramref name="license" /> is <c>null</c> or whitespace.</exception>
        /// <exception cref="XmlException">The license text is not valid XML.</exception>
        /// <exception cref="Exception">The root element is not License.</exception>
        /// <exception cref="Exception">There were no inner nodes found.</exception>
        public IValidationContext ValidateXml(string license)
        {
            var validationContext = new ValidationContext();
            if (string.IsNullOrWhiteSpace(license))
            {
                validationContext.Add(BusinessRuleValidationResult.CreateError("No license available"));
            }

            var xmlDataList = new List<XmlDataModel>();

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(license);
                var xmlRoot = xmlDoc.DocumentElement;
                if (!string.Equals(xmlRoot.Name, "License"))
                {
                    validationContext.Add(BusinessRuleValidationResult.CreateError("Please make sure that you pasted the complete license, including the <License> tags"));
                }

                var xmlNodes = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlNodes)
                {
                    if (!string.Equals(node.Name, "ProductFeatures"))
                    {
                        xmlDataList.Add(new XmlDataModel
                        {
                            Name = node.Name,
                            Value = node.InnerText
                        });
                    }
                    else
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
                }

                if (xmlDataList.Count == 0)
                {
                    validationContext.Add(BusinessRuleValidationResult.CreateError("License contains no valid data"));
                }

                var expData = xmlDataList.FirstOrDefault(x => string.Equals(x.Name, LicenseElements.Expiration));
                if (expData != null)
                {
                    DateTime expirationDateTime;
                    if (DateTime.TryParse(expData.Value, out expirationDateTime))
                    {
                        Log.Debug("Using expiration behavior '{0}'", _expirationBehavior.GetType().Name);

                        var portableLicense = License.Load(license);

                        if (_expirationBehavior.IsExpired(portableLicense, expirationDateTime, DateTime.Now))
                        {
                            validationContext.Add(BusinessRuleValidationResult.CreateError("The license has expired. Please delete the current license if you have a new one."));
                        }
                    }
                    else
                    {
                        validationContext.Add(BusinessRuleValidationResult.CreateError("The expiration date was not a valid date / tim value"));
                    }
                }

                var xmlDataVersion = xmlDataList.FirstOrDefault(x => string.Equals(x.Name, LicenseElements.Version));
                if (xmlDataVersion != null)
                {
                    Version licenseVersion;
                    if (Version.TryParse(xmlDataVersion.Value, out licenseVersion))
                    {
                        var productVersion = Assembly.GetExecutingAssembly().GetName().Version;
                        if (productVersion > licenseVersion)
                        {
                            validationContext.Add(BusinessRuleValidationResult.CreateError("Your license only supports versions up to '{0}' while the current version of this product is '{1}'", licenseVersion, productVersion));
                        }
                    }
                    else
                    {
                        validationContext.Add(BusinessRuleValidationResult.CreateError("The version was not a valid version value"));
                    }
                }
            }
            catch (XmlException xmlex)
            {
                Log.Debug(xmlex);

                validationContext.Add(BusinessRuleValidationResult.CreateError("The license data is not a license"));
            }
            catch (Exception ex)
            {
                Log.Debug(ex);

                validationContext.Add(BusinessRuleValidationResult.CreateError(ex.Message));
            }

            if (validationContext.HasErrors || validationContext.HasWarnings)
            {
                Log.Warning("The XML is invalid");
            }

            return validationContext;
        }

        private void LogLicenseValidity(IValidationContext validationContext)
        {
            if (validationContext.GetErrors().Count > 0)
            {
                Log.Warning("License is not valid:");
                Log.Indent();

                foreach (var error in validationContext.GetErrors())
                {
                    Log.Warning("- {0}\n{1}", error.Message, error.Tag as string);
                }

                Log.Unindent();
            }
            else
            {
                Log.Info("License is valid");
            }
        }
    }
}
