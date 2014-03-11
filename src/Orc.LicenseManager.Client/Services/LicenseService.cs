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
    using System.Reflection;
    using System.Xml;
    using Catel;
    using Catel.Data;
    using Catel.Fody;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.MVVM.Services;
    using Catel.Reflection;
    using Models;
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
        public void Initialize([NotNullOrWhitespace] string applicationId)
        {
            Log.Debug("Initailized"); // TODO: Debug or Info : / ? DILEMA
            _initialized = true;
            _applicationId = applicationId;
        }

        /// <summary>
        /// Shows the single license dialog including all company info. You will see the about box.
        /// </summary>
        /// <param name="aboutTitle">The title inside the about box.</param>
        /// <param name="aboutImae">The about box image.</param>
        /// <param name="aboutText">The text inside the about box</param>
        /// <param name="aboutSite">The site inside the about box.</param>
        /// <param name="title">The title. If <c>null</c>, the title will be extracted from the entry assembly.</param>
        /// <param name="purchaseLink">The url to the store. If <c>null</c>, no purchaseLink link will be displayed.</param>
        /// <exception cref="System.Exception">Please use the Initialize method first</exception>
        /// <exception cref="Exception">The <see cref="Initialize" /> method must be run first.</exception>
        public void ShowSingleLicenseDialog(string aboutTitle, string aboutImae, string aboutText, string aboutSite = null, string title = null, string purchaseLink = null)
        {
            if (!_initialized)
            {
                throw new Exception("Please use the Initialize method first");
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                Assembly assembly = Assembly.GetExecutingAssembly() ?? Assembly.GetEntryAssembly();
                title = assembly.Title();
            }
            var model = new SingleLicenseModel { Title = title, PurchaseLink = purchaseLink, AboutImage = aboutImae, AboutTitle = aboutTitle, AboutText = aboutText, AboutSite = aboutSite };
            var vm = _viewModelFactory.CreateViewModel<SingleLicenseViewModel>(model);
            _uiVisualizerService.ShowDialog(vm);
            Log.Info("Showing dialog with companyinfo");
        }

        /// <summary>
        /// Shows the single license dialog. You won't see the about box.
        /// </summary>
        /// <param name="title">The title. If <c>null</c>, the title will be extracted from the entry assembly.</param>
        /// <param name="purchaseLink">The url to the store. If <c>null</c>, no purchaseLink link will be displayed.</param>
        /// <exception cref="Exception">The <see cref="Initialize" /> method must be run first.</exception>
        public void ShowSingleLicenseDialog(string title = null, string purchaseLink = null)
        {
            if (!_initialized)
            {
                throw new Exception("Please use the Initialize method first");
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                Assembly assembly = Assembly.GetExecutingAssembly() ?? Assembly.GetEntryAssembly();
                title = assembly.Title();
            }

            var model = new SingleLicenseModel { Title = title, PurchaseLink = purchaseLink };
            var vm = _viewModelFactory.CreateViewModel<SingleLicenseViewModel>(model);
            _uiVisualizerService.ShowDialog(vm);
            Log.Info("Showing dialog");
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
                const string error = "Please use the Initialize method first";
                Log.Error(error);
                throw new Exception(error);
            }
            try
            {
                License licenseObject = License.Load(license);
                List<IValidationFailure> failureList = licenseObject.Validate()
                    .Signature(_applicationId)
                    .AssertValidLicense().ToList();
                if (failureList.Count > 0)
                {
                    foreach (IValidationFailure failure in failureList)
                    {
                        BusinessRuleValidationResult businessRuleValidationResult = BusinessRuleValidationResult.CreateErrorWithTag(failure.Message, failure.HowToResolve);
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
                    foreach (IValidationResult error in validationContext.GetErrors())
                    {
                        Log.Warning("- {0}\n{1}", error.Message, error.Tag as string);
                    }
                    Log.Unindent();
                }
            }
            return validationContext;
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
        public IValidationContext ValidateXML(string license)
        {
            // TODO: Ask geert about the exceptions, it can be exception, argument, xml.... mayby more
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
                if (xmlRoot.Name != "License")
                {
                    validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("Please make sure that you pasted the complete xmldata, including the License tag", "Please make sure that you copied the whole text."));
                }
                var xmlNodes = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlNodes)
                {
                    if (node.Name != "ProductFeatures")
                    {
                        xmlDataList.Add(new XmlDataModel()
                        {
                            Name = node.Name,
                            Value = node.InnerText
                        });
                    }
                    else
                    {
                        foreach (XmlNode featrureNode in node.ChildNodes)
                        {
                            xmlDataList.Add(new XmlDataModel()
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
                var expData = xmlDataList.FirstOrDefault(x => x.Name == "Expiration");
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
                var xmlDataVersion = xmlDataList.FirstOrDefault(x => x.Name == "Version");
                if (xmlDataVersion != null)
                {
                    Version licenseVersion;
                    if (Version.TryParse(xmlDataVersion.Value, out licenseVersion))
                    {
                        var productVersion = Assembly.GetExecutingAssembly().GetName().Version;
                        if (productVersion > licenseVersion)
                        {
                            validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("The license has expired.", "Your license only support until version (" + licenseVersion.ToString() + ") while the current version of this product is: (" + productVersion.ToString() + ")."));
                        }
                    }
                    else
                    {
                        validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("The version was no valid Version value", "Please make sure you got a valid license."));

                    }

                }
                Log.Info("the XML is valid.");
            }
            catch (XmlException xmlex)
            {
                Log.Debug(xmlex);
                validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag("The pasted text is not valid XML.", "Please make sure that you copied the whole text."));
                Log.Info("the XML is invalid.");

            }
            catch (Exception ex)
            {
                Log.Debug(ex);
                string innermessage = "";
                if (ex.InnerException != null)
                {
                    innermessage = ex.InnerException.Message;
                }
                validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag(ex.Message, innermessage));
                Log.Info("the XML is invalid.");
            }
            return validationContext;
        }
        /// <summary>
        /// Saves the license.
        /// </summary>
        /// <param name="license">The license key that will be saved to <c>Catel.IO.Path.GetApplicationDataDirectory</c> .</param>
        /// <returns>Returns only true if the license is valid.</returns>
        /// <exception cref="ArgumentException">The <paramref name="license" /> is <c>null</c> or whitespace.</exception>
        public void SaveLicense([NotNullOrWhitespace] string license)
        {
            try
            {
                string xmlFilePath = GetLicenseInfoPath();
                License licenseObject = License.Load(license);
                XmlWriter xmlWriter = XmlWriter.Create(xmlFilePath);
                licenseObject.Save(xmlWriter);
                xmlWriter.Flush();
                xmlWriter.Close();
                Log.Info("License saved.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed To save license!");
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
                Log.Info("License got removed.");
                File.Delete(xmlFilePath);
            }
            else
            {
                Log.Info("License didn't get removed, didn't exist.");
            }
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
                Log.Info("License exist.");
                return true;
            }
            Log.Info("License doesn't exist.");
            return false;
        }

        /// <summary>
        /// Loads the license.
        /// </summary>
        /// <returns>The license from <c>Catel.IO.Path.GetApplicationDataDirectory</c> unless it failed to load then it returns an empty string</returns>
        public string LoadLicense()
        {
            string xmlFilePath = GetLicenseInfoPath();
            try
            {
                XmlReader xmlReader = XmlReader.Create(xmlFilePath);
                License licenseObject = License.Load(xmlReader);
                Log.Info("License loaded: {0}.", licenseObject.ToString());
                return licenseObject.ToString();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Failed to load the license, returning empty string");
                Log.Unindent();
                return "";
            }
        }


        /// <summary>
        /// Loads the XML out of license.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <returns>
        /// A List of with the xml names and values
        /// </returns>
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
                    if (node.Name != "ProductFeatures")
                    {
                        xmlDataList.Add(new XmlDataModel()
                        {
                            Name = node.Name,
                            Value = node.InnerText
                        });
                    }
                    else
                    {
                        foreach (XmlNode featrureNode in node.ChildNodes)
                        {
                            xmlDataList.Add(new XmlDataModel()
                            {
                                Name = featrureNode.Attributes[0].Value,
                                Value = featrureNode.InnerText
                            });
                        }
                    }
                }
                Log.Info("returning xml successfull");
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