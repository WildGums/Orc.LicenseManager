// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleLicenseViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.MVVM.Services;
    using Models;
    using Services;

    /// <summary>
    /// View model for a single License.
    /// </summary>
    public class SingleLicenseViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IProcessService _processService;

        private readonly ILicenseService _licenseService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleLicenseViewModel" /> class.
        /// </summary>
        /// <param name="singleLicenseModel">The single license model.</param>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="processService">The process service.</param>
        /// <param name="licenseService">The license service.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="singleLicenseModel" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="navigationService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="processService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="licenseService" /> is <c>null</c>.</exception>
        public SingleLicenseViewModel(SingleLicenseModel singleLicenseModel, INavigationService navigationService, IProcessService processService, ILicenseService licenseService)
        {
            Argument.IsNotNull(() => singleLicenseModel);
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => processService);
            Argument.IsNotNull(() => licenseService);

            _navigationService = navigationService;
            _processService = processService;
            _licenseService = licenseService;

            SingleLicenseModel = singleLicenseModel;
            WebsiteClick = new Command(OnWebsiteClickExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the WebsiteClick command.
        /// </summary>
        public Command WebsiteClick { get; private set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool FailureOccurred { get; set; }

        /// <summary>
        /// Gets the failure message.
        /// </summary>
        /// <value>
        /// The failure message.
        /// </value>
        public string FailureMessage { get; private set; }

        /// <summary>
        /// Gets the failure solution.
        /// </summary>
        /// <value>
        /// The failure solution.
        /// </value>
        public string FailureSolution { get; private set; }

        [Model]
        [Catel.Fody.Expose("Title")]
        [Catel.Fody.Expose("Website")]
        [Catel.Fody.Expose("Key")]
        [Catel.Fody.Expose("WebsiteIsSet")]
        private SingleLicenseModel SingleLicenseModel { get; set; }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public string Color { get; set; }
        private void OnKeyChanged()
        {
            Log.Debug("Key value has changed.");
            if (_licenseService.ValidateLicense(SingleLicenseModel.Key).HasErrors)
            {
                Color = "Red";
            }
            else
            {
                Color = "Green";
            }
        }
        #region Methods
        /// <summary>
        /// Closes this instance. Always called after the <see cref="M:Catel.MVVM.ViewModelBase.Cancel" /> of <see cref="M:Catel.MVVM.ViewModelBase.Save" /> method.
        /// </summary>
        protected override void Close()
        {
            if (FailureOccurred)
            {
                Log.Debug("Closing application becauso no valid key was given.");
                _navigationService.CloseApplication();
            }
            Log.Debug("Closing dialog with a valid lisence.");
            base.Close();
        }

        /// <summary>
        /// Method to invoke when the WebsiteClick command is executed.
        /// </summary>
        private void OnWebsiteClickExecute()
        {
            _processService.StartProcess(SingleLicenseModel.Website);
        }

        /// <summary>
        /// Initializes the view model. Normally the initialization is done in the constructor, but sometimes this must be delayed
        /// to a state where the associated UI element (user control, window, ...) is actually loaded.
        /// <para />
        /// This method is called as soon as the associated UI element is loaded.
        /// </summary>
        /// <remarks>
        /// It's not recommended to implement the initialization of properties in this method. The initialization of properties
        /// should be done in the constructor. This method should be used to start the retrieval of data from a web service or something
        /// similar.
        /// <para />
        /// During unit tests, it is recommended to manually call this method because there is no external container calling this method.
        /// </remarks>
        protected override void Initialize()
        {
            var pleaseWaitService = DependencyResolver.Resolve<IPleaseWaitService>(); // Once we ll verify online this will be useful
            pleaseWaitService.Show("Checking licenses");
            if (_licenseService.LicenseExists())
            {
                var licenseText = _licenseService.LoadLicense();
                var validationContext = _licenseService.ValidateLicense(licenseText);
                if (validationContext.HasErrors == false)
                {
                    FailureOccurred = false;
                }
                else
                {
                    FailureOccurred = true;
                }
            }
            else
            {
                FailureOccurred = true;
            }
            pleaseWaitService.Hide();
        }

        /// <summary>
        /// Is fired upon pressing the Ok button! Saves the data, closes the window.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if successful; otherwise <c>false</c>.
        /// </returns>
        protected override bool Save()
        {
            var validationContext = _licenseService.ValidateLicense(SingleLicenseModel.Key);
            var firstError = validationContext.GetBusinessRuleErrors().FirstOrDefault();
            if (firstError != null)
            {
                Log.Debug("Ok pressed without a valid key");
                FailureOccurred = true;
                FailureMessage = firstError.Message;
                FailureSolution = firstError.Tag as string;
            }
            else
            {
                Log.Debug("Ok pressed with a valid key");
                FailureOccurred = false;
                _licenseService.SaveLicense(SingleLicenseModel.Key);
            }
            return !validationContext.HasErrors;
        }
        #endregion
    }
}