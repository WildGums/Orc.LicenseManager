// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Catel.Fody;
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
        private List<IValidationFailure> _failures;
        private string _applicationId;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseService" /> class.
        /// </summary>
        /// <param name="uiVisualizerService">The UI visualizer service.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="uiVisualizerService" /> is <c>null</c>.</exception>
        public LicenseService(IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => uiVisualizerService);

            _uiVisualizerService = uiVisualizerService;
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
            _applicationId = applicationId;
        }

        /// <summary>
        /// Shows the single license dialog.
        /// </summary>
        /// <param name="title">The title. If <c>null</c>, the title will be extracted from the entry assembly.</param>
        /// <param name="website">The website.  If <c>null</c>, no website link will be displayed.</param>
        public void ShowSingleLicenseDialog(string title = null, string website = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                var assembly = Assembly.GetExecutingAssembly() ?? Assembly.GetEntryAssembly();
                title = assembly.Title();
            }
            var model = new SingleLicenseModel();
            model.Title = title;
            model.Website = website;
            var vm = new SingleLicenseViewModel(model);
            _uiVisualizerService.ShowDialog(vm);
        }

        /// <summary>
        /// Validates the lisence.
        /// </summary>
        /// <param name="lisence">The lisence key the user has given to be validated.</param>
        /// <returns>Returns only true if the license is valid.</returns>
        /// <exception cref="ArgumentException">The <paramref name="lisence"/> is <c>null</c> or whitespace.</exception>
        public bool ValidateLisence([NotNullOrWhitespace] string lisence)
        {
            try
            {
                var lisenceObject = License.Load(lisence);
                _failures = lisenceObject.Validate().Signature(_applicationId).AssertValidLicense().ToList();
                if (_failures.Count() == 0) // if there are no errors
                {
                    return true;
                }
            }
            catch (Exception)
            {
                _failures = new List<IValidationFailure>();
                _failures.Add(new GeneralValidationFailure()
                {
                    Message = "The given key was in an invalid format.",
                    HowToResolve = "Verify or you copied the whole string into the textfield."
                });
            }
            return false;
        }

        /// <summary>
        /// Gets the validation error.
        /// </summary>
        /// <returns>
        /// An <c>IValidationFailure</c> if the validation failed.
        /// </returns>
        /// <exception cref="System.Exception">Please try to validate the lisence first.</exception>
        public IValidationFailure GetValidationError()
        {
            if (_failures == null)
            {
                throw new Exception("Please Validate first.");
            }
            if (_failures.Count() == 0)
            {
                throw new Exception("There were no issues with validating the lisence");
            }
            try
            {
                return _failures.First();
            }
            catch (Exception)
            {
                // I don t want to have to do this :D
                return _failures.First();
            }
        }
        #endregion
    }
}