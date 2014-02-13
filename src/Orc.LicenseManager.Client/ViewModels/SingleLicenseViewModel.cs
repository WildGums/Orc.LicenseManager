// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleLicenseViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
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
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

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
            NoFailure = true;
            XmlData = new ObservableCollection<XMLDataModel>();

            SingleLicenseModel = singleLicenseModel;
            WebsiteClick = new Command(OnWebsiteClickExecute);
            Paste = new Command(OnPasteExecute);
            Exit = new Command(OnExitExecute);
            ShowClipboard = new Command(OnShowClipboardExecute);
            Submit = new Command(OnSubmitExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the Submit command.
        /// </summary>
        public Command Submit { get; private set; }

        /// <summary>
        /// Gets the Exit command.
        /// </summary>
        public Command Exit { get; private set; }

        /// <summary>
        /// Gets the Close command.
        /// </summary>
        /// <summary>
        /// Gets the WebsiteClick command.
        /// </summary>
        public Command WebsiteClick { get; private set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool FailureOccurred { get; set; }

        /// <summary>
        /// Gets set by the opposite way of FailureOccured
        /// </summary>
        public bool NoFailure { get; set; }

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

        /// <summary>
        /// Color is either red or green when successful
        /// </summary>
        [DefaultValue("Red")]
        public string Color { get; set; }

        /// <summary>
        /// Gets the Paste command.
        /// </summary>
        public Command Paste { get; private set; }

        /// <summary>
        /// List of xml Data, only populated when license was valid.
        /// </summary>
        public ObservableCollection<XMLDataModel> XmlData { get; set; }

        /// <summary>
        /// Gets the ShowClipboard command.
        /// </summary>
        public Command ShowClipboard { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the Submit command is executed.
        /// </summary>
        private void OnSubmitExecute()
        {
            _licenseService.SaveLicense(SingleLicenseModel.Key);
            base.Close(); // TODO: Fix this :/
        }



        /// <summary>
        /// Method to invoke when the Exit command is executed.
        /// </summary>
        private void OnExitExecute()
        {
            Log.Debug("Closing application.");
            _navigationService.CloseApplication();

        }

        private void OnFailureOccurredChanged()
        {
            NoFailure = !FailureOccurred;
            Log.Debug("NoFailure is: " + NoFailure);
            Log.Debug("FailureOccurred is: " + FailureOccurred);
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
        /// Method to invoke when the Paste command is executed. Validates the lisence and xml, 
        /// </summary>
        private void OnPasteExecute()
        {
            if (FailureOccurred == true)
            {
                XmlData.Clear();
                if (Clipboard.GetText() != string.Empty)
                {
                    SingleLicenseModel.Key = Clipboard.GetText();
                    string lisence = SingleLicenseModel.Key;
                    var xmlFirstError = _licenseService.ValidateXML(lisence).GetBusinessRuleErrors().FirstOrDefault();
                    if (xmlFirstError == null)
                    {
                        var normalFirstError = _licenseService.ValidateLicense(SingleLicenseModel.Key).GetBusinessRuleErrors().FirstOrDefault();
                        if (normalFirstError == null)
                        {
                            var xmlList = _licenseService.LoadXMLFromLisence(SingleLicenseModel.Key);
                            xmlList.ForEach(XmlData.Add);
                            FailureOccurred = false;
                        }
                        else
                        {
                            FailureMessage = normalFirstError.Message;
                            FailureSolution = normalFirstError.Tag as string;
                        }
                    }
                    else
                    {
                        FailureMessage = xmlFirstError.Message;
                        FailureSolution = xmlFirstError.Tag as string;
                    }
                }
                else
                {
                    FailureMessage = "No text was pasted into the windows.";
                    FailureSolution = "Please copy the license text into this window.";
                }
            }
        }

        /// <summary>
        /// Method to invoke when the ShowClipboard command is executed.
        /// </summary>
        private void OnShowClipboardExecute()
        {
            //TODO: Show clipboard
        }
        #endregion
    }
}