// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleLicenseViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Models;
    using Services;

    /// <summary>
    /// View model for a single License.
    /// </summary>
    public class LicenseViewModel : ViewModelBase
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IProcessService _processService;

        private readonly ILicenseService _licenseService;

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IMessageService _messageService;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseViewModel" /> class.
        /// </summary>
        /// <param name="licenseInfo">The single license model.</param>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="processService">The process service.</param>
        /// <param name="licenseService">The license service.</param>
        /// <param name="uiVisualizerService">The uiVisualizer service.</param>
        /// <param name="viewModelFactory">The uiVisualizer service.</param>
        /// <param name="messageService"></param>
        /// <exception cref="ArgumentNullException">The <paramref name="licenseInfo" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="navigationService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="processService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="licenseService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="uiVisualizerService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="viewModelFactory" /> is <c>null</c>.</exception>
        public LicenseViewModel(LicenseInfo licenseInfo, INavigationService navigationService, IProcessService processService,
            ILicenseService licenseService, IUIVisualizerService uiVisualizerService, IViewModelFactory viewModelFactory,
            IMessageService messageService)
        {
            Argument.IsNotNull(() => licenseInfo);
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => processService);
            Argument.IsNotNull(() => licenseService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => viewModelFactory);
            Argument.IsNotNull(() => messageService);

            _navigationService = navigationService;
            _processService = processService;
            _licenseService = licenseService;
            _uiVisualizerService = uiVisualizerService;
            _viewModelFactory = viewModelFactory;
            _messageService = messageService;

            LicenseInfo = licenseInfo;
            Title = licenseInfo.Title;
            LicenseExists = _licenseService.LicenseExists();

            XmlData = new ObservableCollection<XmlDataModel>();

            Paste = new Command(OnPasteExecute);
            ShowClipboard = new Command(OnShowClipboardExecute);
            PurchaseLinkClick = new Command(OnPurchaseLinkClickExecute);
            AboutSiteClick = new Command(OnAboutSiteClickExecute);
            RemoveLicense = new Command(OnRemoveLicenseExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the Close command.
        /// </summary>
        /// <summary>
        /// Gets the WebsiteClick command.
        /// </summary>
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

        /// <summary>
        /// Gets the PurchaseLinkClick command.
        /// </summary>
        public Command PurchaseLinkClick { get; private set; }

        /// <summary>
        /// Gets the AboutSiteClick command.
        /// </summary>
        public Command AboutSiteClick { get; private set; }

        /// <summary>
        /// Gets the RemoveLicense command.
        /// </summary>
        public Command RemoveLicense { get; private set; }

        [Model]
        [Catel.Fody.Expose("PurchaseUrl")]
        [Catel.Fody.Expose("InfoUrl")]
        [Catel.Fody.Expose("Text")]
        [Catel.Fody.Expose("ImageUri")]
        [Catel.Fody.Expose("Key")]
        private LicenseInfo LicenseInfo { get; set; }

        /// <summary>
        /// Gets the about image URI.
        /// </summary>
        /// <value>
        /// The about image URI.
        /// </value>
        public Uri AboutImageUri { get { return new Uri(LicenseInfo.ImageUri, UriKind.RelativeOrAbsolute); } }

        /// <summary>
        /// Gets the Paste command.
        /// </summary>
        public Command Paste { get; private set; }

        /// <summary>
        /// List of xml Data, only populated when license was valid.
        /// </summary>
        public ObservableCollection<XmlDataModel> XmlData { get; set; }

        /// <summary>
        /// Gets the ShowClipboard command.
        /// </summary>
        public Command ShowClipboard { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [paste success].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [paste success]; otherwise, <c>false</c>.
        /// </value>
        public bool PasteSuccess { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show failure].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show failure]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowFailure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [license exists].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [license exists]; otherwise, <c>false</c>.
        /// </value>
        public bool LicenseExists { get; private set; }

        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the AboutSiteClick command is executed.
        /// </summary>
        private void OnAboutSiteClickExecute()
        {
            _processService.StartProcess(LicenseInfo.InfoUrl);
        }

        /// <summary>
        /// Method to invoke when the PurchaseLinkClick command is executed.
        /// </summary>
        private void OnPurchaseLinkClickExecute()
        {
            _processService.StartProcess(LicenseInfo.PurchaseUrl);
        }

        /// <summary>
        /// Method to invoke when the RemoveLicense command is executed.
        /// </summary>
        private async void OnRemoveLicenseExecute()
        {
            if (await _messageService.Show("Are you sure you want to delete the existing license ?", "Delete existing license ?", MessageButton.YesNo,
                MessageImage.Question) == MessageResult.Yes)
            {
                _licenseService.RemoveLicense();

                UpdateLicenseInfo();
            }
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
        protected override async Task Initialize()
        {
            UpdateLicenseInfo();
        }

        protected override async Task<bool> Save()
        {
            if (_licenseService.LicenseExists())
            {
                return true;
            }

            var validationContext = _licenseService.ValidateLicense(LicenseInfo.Key);
            if (validationContext.HasErrors)
            {
                return false;
            }

            _licenseService.SaveLicense(LicenseInfo.Key);

            return true;
        }

        protected override async Task<bool> Cancel()
        {
            if (!_licenseService.LicenseExists())
            {
                Log.Debug("Closing application");

                _navigationService.CloseApplication();
            }

            return true;
        }

        private void UpdateLicenseInfo()
        {
            var licenseText = _licenseService.LoadLicense();
            ApplyLicense(licenseText);
        }

        private void ApplyLicense(string licenseKey)
        {
            LicenseExists = false;
            XmlData.Clear();
            RaisePropertyChanged(() => XmlData);

            if (string.IsNullOrWhiteSpace(licenseKey))
            {
                FailureOccurred = false;
                ShowFailure = false;
                return;
            }

            XmlData.Clear();

            LicenseInfo.Key = licenseKey;

            var xmlList = _licenseService.LoadXmlFromLicense(LicenseInfo.Key);
            xmlList.ForEach(x =>
            {
                if (string.Equals(x.Name, "Expiration"))
                {
                    x.Name = "Maintenance End Date";
                }

                XmlData.Add(x);
            });

            var validationContext = _licenseService.ValidateLicense(licenseKey);
            if (validationContext.HasErrors)
            {
                var xmlFirstError = _licenseService.ValidateXml(licenseKey).GetBusinessRuleErrors().FirstOrDefault();
                if (xmlFirstError == null)
                {
                    var normalFirstError = _licenseService.ValidateLicense(LicenseInfo.Key).GetBusinessRuleErrors().FirstOrDefault();
                    if (normalFirstError == null)
                    {

                    }
                    else
                    {
                        ShowFailure = true;
                        FailureMessage = normalFirstError.Message;
                        FailureSolution = normalFirstError.Tag as string;
                    }
                }
                else
                {
                    ShowFailure = true;
                    FailureMessage = xmlFirstError.Message;
                    FailureSolution = xmlFirstError.Tag as string;
                }
            }
            else
            {
                FailureOccurred = false;
                ShowFailure = false;
            }

            LicenseExists = true;
            RaisePropertyChanged(() => XmlData);
        }

        /// <summary>
        /// Method to invoke when the Paste command is executed. Validates the license and xml, 
        /// </summary>
        private void OnPasteExecute()
        {
            if (Clipboard.GetText() != string.Empty)
            {
                var license = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(license))
                {
                    ShowFailure = true;

                    // TODO: Read from resources (language service preferred)
                    FailureMessage = "No text was pasted into the window.";
                    FailureSolution = "Please copy the license text into this window.";
                    return;
                }

                ApplyLicense(license);
            }
        }

        /// <summary>
        /// Method to invoke when the ShowClipboard command is executed.
        /// </summary>
        private void OnShowClipboardExecute()
        {
            _uiVisualizerService.ShowDialog<ClipBoardViewModel>();
        }
        #endregion
    }
}