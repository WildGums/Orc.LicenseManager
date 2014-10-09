// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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
    using System.Threading.Tasks;
    using System.Web;
    using System.Xml;
    using Catel;
    using Catel.Data;
    using Catel.Fody;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Reflection;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Portable.Licensing;
    using Portable.Licensing.Validation;
    using ViewModels;

    /// <summary>
    /// Service to validate, store and remove licenses for software products.
    /// </summary>
    public class LicenseService : ILicenseService
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IViewModelFactory _viewModelFactory;

        private string _applicationId;
        private bool _initialized;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseService" /> class.
        /// </summary>
        /// <param name="uiVisualizerService">The UI visualizer service.</param>
        /// <param name="viewModelFactory">The view model factory.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="uiVisualizerService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="viewModelFactory" /> is <c>null</c>.</exception>
        public LicenseService(IUIVisualizerService uiVisualizerService, IViewModelFactory viewModelFactory)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => viewModelFactory);

            _uiVisualizerService = uiVisualizerService;
            _viewModelFactory = viewModelFactory;
        }
        #endregion

        #region ILicenseService Members
        /// <summary>
        /// Initializes the specified application identifier.
        /// </summary>
        /// <param name="applicationId">The application identifier.</param>
        /// <exception cref="ArgumentException">The <paramref name="applicationId" /> is <c>null</c> or whitespace.</exception>
        public async Task Initialize(string applicationId)
        {
            Argument.IsNotNull(() => applicationId);

            _initialized = true;
            _applicationId = applicationId;
        }

        /// <summary>
        /// Shows the single license dialog including all company info. You will see the about box.
        /// </summary>
        /// <param name="aboutTitle">The title inside the about box.</param>
        /// <param name="aboutImage">The about box image.</param>
        /// <param name="aboutText">The text inside the about box</param>
        /// <param name="aboutSiteUrl">The site inside the about box.</param>
        /// <param name="title">The title. If <c>null</c>, the title will be extracted from the entry assembly.</param>
        /// <param name="purchaseLinkUrl">The url to the store. If <c>null</c>, no purchaseLinkUrl link will be displayed.</param>
        /// <exception cref="System.Exception">Please use the Initialize method first</exception>
        /// <exception cref="Exception">The <see cref="Initialize" /> method must be run first.</exception>
        public async Task ShowSingleLicenseDialog(string aboutTitle, string aboutImage, string aboutText, string aboutSiteUrl = null, string title = null, string purchaseLinkUrl = null)
        {
            if (!_initialized)
            {
                Log.ErrorAndThrowException<InvalidOperationException>("Please use the Initialize method first");
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                var assembly = AssemblyHelper.GetEntryAssembly();
                title = assembly.Title();
            }

            var model = new SingleLicenseModel
            {
                Title = title,
                PurchaseLink = purchaseLinkUrl,
                AboutImage = aboutImage,
                AboutTitle = aboutTitle,
                AboutText = aboutText,
                AboutSite = aboutSiteUrl
            };

            var vm = _viewModelFactory.CreateViewModel<SingleLicenseViewModel>(model);
            await _uiVisualizerService.ShowDialog(vm);

            Log.Info("Showing license dialog with companyinfo");
        }

        /// <summary>
        /// Shows the single license dialog. You won't see the about box.
        /// </summary>
        /// <param name="title">The title. If <c>null</c>, the title will be extracted from the entry assembly.</param>
        /// <param name="purchaseLink">The url to the store. If <c>null</c>, no purchaseLinkUrl link will be displayed.</param>
        /// <exception cref="Exception">The <see cref="Initialize" /> method must be run first.</exception>
        public async Task ShowSingleLicenseDialog(string title = null, string purchaseLink = null)
        {
            if (!_initialized)
            {
                Log.ErrorAndThrowException<InvalidOperationException>("Please use the Initialize method first");
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                var assembly = AssemblyHelper.GetEntryAssembly();
                title = assembly.Title();
            }

            var model = new SingleLicenseModel
            {
                Title = title,
                PurchaseLink = purchaseLink
            };

            var vm = _viewModelFactory.CreateViewModel<SingleLicenseViewModel>(model);
            await _uiVisualizerService.ShowDialog(vm);

            Log.Info("Showing license dialog");
        }

        /// <summary>
        /// Validates the license.
        /// </summary>
        /// <param name="license">The license key the user has given to be validated.</param>
        /// <returns>
        /// The validation context containing all the validation results.
        /// </returns>
        /// <exception cref="System.Exception">Please use the Initialize method first</exception>
        /// <exception cref="Exception">The <see cref="Initialize" /> method must be run first.</exception>
        public IValidationContext ValidateLicense(string license)
        {
            var validationContext = new ValidationContext();
            if (!_initialized)
            {
                Log.ErrorAndThrowException<InvalidOperationException>("Please use the Initialize method first");
            }

            Log.Info("Validating license");

            try
            {
                var licenseObject = License.Load(license);
                var failureList = licenseObject.Validate()
                    .Signature(_applicationId)
                    .AssertValidLicense().ToList();
                if (failureList.Count > 0)
                {
                    foreach (var failure in failureList)
                    {
                        var businessRuleValidationResult = BusinessRuleValidationResult.CreateErrorWithTag(failure.Message, failure.HowToResolve);
                        validationContext.AddBusinessRuleValidationResult(businessRuleValidationResult);
                    }
                }
            }
            catch (Exception)
            {
                validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("The given key was in an invalid format", "Please check or you copied the whole key."));
            }
            finally
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

            return validationContext;
        }

        /// <summary>
        /// Validates the license on the server.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <param name="serverUrl">The server URL.</param>
        /// <param name="assembly">The assembly to get the information from. If <c>null</c>, the entry assembly will be used.</param>
        /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
        public async Task<LicenseValidationResult> ValidateLicenseOnServer(string license, string serverUrl, Assembly assembly = null)
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
                    var serviceLicenseValidation = new ServerLicenseValidation
                    {
                        ProductName = (assembly != null) ? assembly.Product() : "unknown product (assembly null)",
                        ProductVersion = (assembly != null) ? assembly.Version() : "unknown version (assembly null)",
                        License = license
                    };

                    var json = JsonConvert.SerializeObject(serviceLicenseValidation);

                    sw.Write(json);
                }

                using (var httpWebResponse = await webRequest.GetResponseAsync())
                {
                    using (var responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (var streamReader = new StreamReader(responseStream))
                        {
                            var json = await streamReader.ReadToEndAsync();
                            validationResult = JsonConvert.DeserializeObject<LicenseValidationResult>(json);
                        }
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
                    IsValid = false,
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
                validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("Your clipboard seems to be empty", "Please make sure that you copied the whole text."));
            }

            var xmlDataList = new List<XmlDataModel>();

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(license);
                var xmlRoot = xmlDoc.DocumentElement;
                if (!string.Equals(xmlRoot.Name, "License"))
                {
                    validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("Please make sure that you pasted the complete xmldata, including the License tag", "Please make sure that you copied the whole text."));
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
                        foreach (XmlNode featrureNode in node.ChildNodes)
                        {
                            xmlDataList.Add(new XmlDataModel
                            {
                                Name = featrureNode.Attributes[0].Value,
                                Value = featrureNode.InnerText
                            });
                        }
                    }
                }

                if (xmlDataList.Count == 0)
                {
                    validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("There was no inner XML found", "Please make sure that you copied the whole text."));
                }

                var expData = xmlDataList.FirstOrDefault(x => string.Equals(x.Name, "Expiration"));
                if (expData != null)
                {
                    DateTime expDataDate;
                    if (DateTime.TryParse(expData.Value, out expDataDate))
                    {
                        if (expDataDate < DateTime.UtcNow)
                        {
                            validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("The license has expired.", "Please make sure you got a license that isn't expired."));
                        }
                    }
                    else
                    {
                        validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("The expiration date was no valid DateTime value", "Please make sure you got a valid license."));
                    }
                }

                var xmlDataVersion = xmlDataList.FirstOrDefault(x => string.Equals(x.Name, "Version"));
                if (xmlDataVersion != null)
                {
                    Version licenseVersion;
                    if (Version.TryParse(xmlDataVersion.Value, out licenseVersion))
                    {
                        var productVersion = Assembly.GetExecutingAssembly().GetName().Version;
                        if (productVersion > licenseVersion)
                        {
                            validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("The license has expired.", "Your license only support until version (" + licenseVersion + ") while the current version of this product is: (" + productVersion + ")."));
                        }
                    }
                    else
                    {
                        validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("The version was no valid Version value", "Please make sure you got a valid license."));
                    }
                }

                Log.Info("The XML is invalid");
            }
            catch (XmlException xmlex)
            {
                Log.Debug(xmlex);

                validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("The pasted text is not valid XML.", "Please make sure that you copied the whole text."));

                Log.Info("The XML is invalid");
            }
            catch (Exception ex)
            {
                Log.Debug(ex);

                string innermessage = string.Empty;
                if (ex.InnerException != null)
                {
                    innermessage = ex.InnerException.Message;
                }

                validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag(ex.Message, innermessage));

                Log.Info("The XML is invalid");
            }

            return validationContext;
        }

        /// <summary>
        /// Saves the license.
        /// </summary>
        /// <param name="license">The license key that will be saved to <c>Catel.IO.Path.GetApplicationDataDirectory</c> .</param>
        /// <returns>Returns only true if the license is valid.</returns>
        /// <exception cref="ArgumentException">The <paramref name="license" /> is <c>null</c> or whitespace.</exception>
        public void SaveLicense(string license)
        {
            Argument.IsNotNullOrWhitespace("license", license);

            try
            {
                string xmlFilePath = GetLicenseInfoPath();
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
        public void RemoveLicense()
        {
            string xmlFilePath = GetLicenseInfoPath();

            if (File.Exists(xmlFilePath))
            {
                File.Delete(xmlFilePath);
            }

            Log.Info("License has been removed");
        }

        /// <summary>
        /// Check if the license exists.
        /// </summary>
        /// <returns>returns <c>true</c> if exists else <c>false</c></returns>
        public bool LicenseExists()
        {
            string xmlFilePath = GetLicenseInfoPath();
            if (File.Exists(xmlFilePath))
            {
                Log.Info("License exists");
                return true;
            }

            Log.Info("License does not exist");

            return false;
        }

        /// <summary>
        /// Loads the license.
        /// </summary>
        /// <returns>The license from <c>Catel.IO.Path.GetApplicationDataDirectory</c> unless it failed to load then it returns an empty string</returns>
        public string LoadLicense()
        {
            try
            {
                string xmlFilePath = GetLicenseInfoPath();

                using (var xmlReader = XmlReader.Create(xmlFilePath))
                {
                    var licenseObject = License.Load(xmlReader);
                    Log.Info("License loaded: {0}", licenseObject.ToString());
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
                        foreach (XmlNode featrureNode in node.ChildNodes)
                        {
                            xmlDataList.Add(new XmlDataModel
                            {
                                Name = featrureNode.Attributes[0].Value,
                                Value = featrureNode.InnerText
                            });
                        }
                    }
                }

                Log.Info("Returning xml successfull");
            }
            catch (Exception ex)
            {
                Log.Debug(ex);
                throw;
            }

            return xmlDataList;
        }
        #endregion

        #region Methods
        private string GetLicenseInfoPath()
        {
            return Path.Combine(Catel.IO.Path.GetApplicationDataDirectory(), "LicenseInfo.xml");
        }
        #endregion
    }
}