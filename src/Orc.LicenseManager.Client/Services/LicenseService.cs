// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using Catel;
    using Catel.Data;
    using Catel.Fody;
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
        /// <exception cref="ArgumentException">The <paramref name="applicationId"/> is <c>null</c> or whitespace.</exception>
        public void Initialize([NotNullOrWhitespace] string applicationId)
        {
            _initialized = true;
            _applicationId = applicationId;
        }

        /// <summary>
        /// Shows the single license dialog.
        /// </summary>
        /// <param name="title">The title. If <c>null</c>, the title will be extracted from the entry assembly.</param>
        /// <param name="website">The website. If <c>null</c>, no website link will be displayed.</param>
        /// <exception cref="Exception">The <see cref="Initialize"/> method must be run first.</exception>
        public void ShowSingleLicenseDialog(string title = null, string website = null)
        {
            if (!_initialized)
            {
                throw new Exception("Please use the Initialize method first");
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                var assembly = Assembly.GetExecutingAssembly() ?? Assembly.GetEntryAssembly();
                title = assembly.Title();
            }

            var model = new SingleLicenseModel {Title = title, Website = website};
            var vm = _viewModelFactory.CreateViewModel<SingleLicenseViewModel>(model);

            _uiVisualizerService.ShowDialog(vm);
        }

        /// <summary>
        /// Validates the lisence.
        /// </summary>
        /// <param name="license">The lisence key the user has given to be validated.</param>
        /// <returns>
        /// The validation context containing all the validation results.
        /// </returns>
        /// <exception cref="System.Exception">Please use the Initialize method first</exception>
        /// <exception cref="Exception">The <see cref="Initialize"/> method must be run first.</exception>
        public IValidationContext ValidateLicense(string license)
        {
            var validationContext = new ValidationContext();

            if (!_initialized)
            {
                throw new Exception("Please use the Initialize method first");
            }

            try
            {
                var licenseObject = License.Load(license);

                foreach (var failure in licenseObject.Validate().Signature(_applicationId).AssertValidLicense().ToList())
                {
                    validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateErrorWithTag(failure.Message, failure.HowToResolve));
                }
            }
            catch (Exception)
            {
                validationContext.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateError("The given key was in an invalid format", "Do it again"));
            }

            return validationContext;
        }

        ///// <summary>
        ///// Gets the validation error.
        ///// </summary>
        ///// <returns>
        ///// An <c>IValidationFailure</c> if the validation failed.
        ///// </returns>
        ///// <exception cref="System.Exception">Please try to validate the lisence first.</exception>
        ///// <exception cref="Exception">The <see cref="Initialize"/> method must be run first.</exception>
        //public IValidationFailure GetValidationError()
        //{
        //    if (!_initialized)
        //    {
        //        throw new Exception("Please use the Initialize method first");
        //    }
        //    if (_failures == null)
        //    {
        //        throw new Exception("Please Validate first.");
        //    }
        //    if (_failures.Count() == 0)
        //    {
        //        throw new Exception("There were no issues with validating the lisence");
        //    }
        //    try
        //    {
        //        return _failures.First();
        //    }
        //    catch (Exception)
        //    {
        //        // I don t want to have to do this :D
        //        return _failures.First();
        //    }
        //}

        /// <summary>
        /// Saves the license.
        /// </summary>
        /// <param name="license">The lisence key that will be saved to <c>Catel.IO.Path.GetApplicationDataDirectory</c> .</param>
        /// <returns>Returns only true if the license is valid.</returns>
        /// <exception cref="ArgumentException">The <paramref name="license"/> is <c>null</c> or whitespace.</exception>
        public void SaveLicense([NotNullOrWhitespace] string license)
        {
            var xmlFilePath = GetLicenseInfoPath();
            var licenseObject = License.Load(license);
            var xmlWriter = XmlWriter.Create(xmlFilePath);
            licenseObject.Save(xmlWriter);
            xmlWriter.Flush();
            xmlWriter.Close();
        }

        /// <summary>
        /// Removes the license if exists.
        /// </summary>
        public void RemoveLicense()
        {
            var xmlFilePath = GetLicenseInfoPath();
            if (File.Exists(xmlFilePath))
            {
                File.Delete(xmlFilePath);
            }
        }

        /// <summary>
        /// Check if the license exists.
        /// </summary>
        /// <returns>returns <c>true</c> if exists else <c>false</c></returns>
        public bool LicenseExists()
        {
            var xmlFilePath = GetLicenseInfoPath();

            if (File.Exists(xmlFilePath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loads the license.
        /// </summary>
        /// <returns>The lisence from <c>Catel.IO.Path.GetApplicationDataDirectory</c> unless it failed to load then it returns an empty string</returns>
        public string LoadLicense()
        {
            var xmlFilePath = GetLicenseInfoPath();

            try
            {
                var xmlReader = XmlReader.Create(xmlFilePath);
                var licenseObject = License.Load(xmlReader);
                return licenseObject.ToString();
            }
            catch (Exception)
            {
                return "";
            }
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