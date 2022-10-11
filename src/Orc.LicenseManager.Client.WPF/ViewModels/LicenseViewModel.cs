﻿namespace Orc.LicenseManager.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;

    /// <summary>
    /// View model for a single License.
    /// </summary>
    public class LicenseViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly INavigationService _navigationService;
        private readonly IProcessService _processService;

        private readonly ILicenseService _licenseService;
        private readonly ILicenseValidationService _licenseValidationService;

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;
        private readonly ILanguageService _languageService;
        private readonly ILicenseModeService _licenseModeService;

        public LicenseViewModel(LicenseInfo licenseInfo, INavigationService navigationService, IProcessService processService,
            ILicenseService licenseService, ILicenseValidationService licenseValidationService, IUIVisualizerService uiVisualizerService, 
            IMessageService messageService, ILanguageService languageService, ILicenseModeService licenseModeService)
        {
            ArgumentNullException.ThrowIfNull(licenseInfo);
            ArgumentNullException.ThrowIfNull(navigationService);
            ArgumentNullException.ThrowIfNull(processService);
            ArgumentNullException.ThrowIfNull(licenseService);
            ArgumentNullException.ThrowIfNull(licenseValidationService);
            ArgumentNullException.ThrowIfNull(uiVisualizerService);
            ArgumentNullException.ThrowIfNull(messageService);
            ArgumentNullException.ThrowIfNull(languageService);
            ArgumentNullException.ThrowIfNull(licenseModeService);

            _navigationService = navigationService;
            _processService = processService;
            _licenseService = licenseService;
            _licenseValidationService = licenseValidationService;
            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;
            _languageService = languageService;
            _licenseModeService = licenseModeService;

            LicenseInfo = licenseInfo;
            Title = licenseInfo.Title;

            XmlData = new ObservableCollection<XmlDataModel>();

            Paste = new TaskCommand(OnPasteExecuteAsync);
            ShowClipboard = new TaskCommand(OnShowClipboardExecuteAsync);
            PurchaseLinkClick = new Command(OnPurchaseLinkClickExecute);
            AboutSiteClick = new Command(OnAboutSiteClickExecute);
            RemoveLicense = new TaskCommand(OnRemoveLicenseExecuteAsync, OnRemoveLicenseCanExecute);
        }

        public List<LicenseMode> AvailableLicenseModes { get; private set; } = new List<LicenseMode>();

        public LicenseMode LicenseMode { get; set; }

        public bool FailureOccurred { get; set; }

        /// <summary>
        /// Gets the failure message.
        /// </summary>
        /// <value>The failure message.</value>
        public string? FailureMessage { get; private set; }

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
        public TaskCommand RemoveLicense { get; private set; }

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
        public TaskCommand Paste { get; private set; }

        /// <summary>
        /// List of xml Data, only populated when license was valid.
        /// </summary>
        public ObservableCollection<XmlDataModel> XmlData { get; set; }

        /// <summary>
        /// Gets the ShowClipboard command.
        /// </summary>
        public TaskCommand ShowClipboard { get; private set; }

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

        private async void OnLicenseModeChanged()
        {
            await LoadAndApplyLicenseAsync();
        }

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
        private async Task OnRemoveLicenseExecuteAsync()
        {
            if (await _messageService.ShowAsync("Are you sure you want to delete the existing license?", "Delete existing license?", MessageButton.YesNo,
                MessageImage.Question) == MessageResult.Yes)
            {
                _licenseService.RemoveLicense(LicenseMode);
                await UpdateLicenseInfoAsync();
            }
        }

        private bool OnRemoveLicenseCanExecute()
        {
            return _licenseService.LicenseExists(LicenseMode);
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
        protected override async Task InitializeAsync()
        {
            AvailableLicenseModes = _licenseModeService.GetAvailableLicenseModes();

            await UpdateLicenseInfoAsync();
        }

        protected override async Task<bool> SaveAsync()
        {
            var licenseExists = _licenseService.LicenseExists(LicenseMode);
            
            var oppositeLicenseMode = LicenseMode.ToOpposite();
            
            var oppositeLicenseExists = _licenseService.LicenseExists(oppositeLicenseMode);

            if (licenseExists && !oppositeLicenseExists)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(LicenseInfo.Key))
            {
                return false;
            }

            var validationContext = await _licenseValidationService.ValidateLicenseAsync(LicenseInfo.Key);
            if (validationContext.HasErrors)
            {
                return false;
            }

            try
            {
                _licenseService.SaveLicense(LicenseInfo.Key, LicenseMode);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to save license using '{LicenseMode}'");

                await _messageService.ShowErrorAsync(_languageService.GetRequiredString("FailedToSaveLicense"));
                return false;
            }

            if (oppositeLicenseExists)
            {
                var messageResult = await _messageService.ShowAsync(string.Format("The license for {0} has been successfully created, would you like to remove license for {1}?",
                    LicenseMode.ToDescriptionText(), oppositeLicenseMode.ToDescriptionText()), "Remove license confirmation", MessageButton.YesNo);

                if (messageResult == MessageResult.Yes)
                {
                    _licenseService.RemoveLicense(oppositeLicenseMode);
                }
            }

            return true;
        }

        protected override async Task<bool> CancelAsync()
        {
            if (!_licenseService.AnyExistingLicense())
            {
                Log.Debug("Closing application");

                await _navigationService.CloseApplicationAsync();
            }

            return true;
        }

        private async Task UpdateLicenseInfoAsync()
        {
            DetectLicenseMode();

            await LoadAndApplyLicenseAsync();
        }

        private async Task LoadAndApplyLicenseAsync()
        {
            var licenseText = _licenseService.LoadLicense(LicenseMode);
            if (string.IsNullOrWhiteSpace(licenseText))
            {
                licenseText = _licenseService.LoadLicense(LicenseMode.ToOpposite());
            }

            await ApplyLicenseAsync(licenseText);
        }

        private void DetectLicenseMode()
        {
            LicenseExists = _licenseService.LicenseExists(LicenseMode.CurrentUser);
            LicenseMode = LicenseMode.CurrentUser;

            if (!LicenseExists && _licenseService.LicenseExists(LicenseMode.MachineWide))
            {
                LicenseExists = true;
                LicenseMode = LicenseMode.MachineWide;
            }
        }

        private async Task ApplyLicenseAsync(string licenseKey)
        {
            LicenseExists = false;
            XmlData.Clear();
            RaisePropertyChanged(nameof(XmlData));

            LicenseInfo.Key = licenseKey;

            if (string.IsNullOrWhiteSpace(licenseKey))
            {
                FailureOccurred = false;
                ShowFailure = false;
                return;
            }

            XmlData.Clear();

            var xmlList = _licenseService.LoadXmlFromLicense(LicenseInfo.Key);
            if (xmlList is null)
            {
                FailureOccurred = false;
                ShowFailure = false;
                return;
            }

            xmlList.ForEach(x =>
            {
                if (string.Equals(x.Name, "Expiration"))
                {
                    x.Name = "Maintenance End Date";
                }

                XmlData.Add(x);
            });

            var validationContext = await _licenseValidationService.ValidateLicenseAsync(licenseKey);
            if (validationContext.HasErrors)
            {
                var xmlValidationContext = await _licenseValidationService.ValidateXmlAsync(licenseKey);

                var xmlFirstError = xmlValidationContext.GetBusinessRuleErrors().FirstOrDefault();
                if (xmlFirstError is null)
                {
                    var normalValidationContext = await _licenseValidationService.ValidateLicenseAsync(LicenseInfo.Key);
                    var normalFirstError = normalValidationContext.GetBusinessRuleErrors().FirstOrDefault();
                    if (normalFirstError is not null)
                    {
                        ShowFailure = true;
                        FailureMessage = normalFirstError.Message;
                    }
                }
                else
                {
                    ShowFailure = true;
                    FailureMessage = xmlFirstError.Message;
                }
            }
            else
            {
                FailureOccurred = false;
                ShowFailure = false;
            }

            LicenseExists = true;
            RaisePropertyChanged(nameof(XmlData));
        }

        /// <summary>
        /// Method to invoke when the Paste command is executed. Validates the license and xml, 
        /// </summary>
        private async Task OnPasteExecuteAsync()
        {
            if (Clipboard.GetText() != string.Empty)
            {
                var license = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(license))
                {
                    ShowFailure = true;

                    // TODO: Read from resources (language service preferred)
                    FailureMessage = LanguageHelper.GetString("NoTextWasPasted");
                    return;
                }

                await ApplyLicenseAsync(license);

                if (!string.IsNullOrEmpty(LicenseInfo.Key))
                {
                    if (LicenseExists)
                    {
                        await SaveAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Method to invoke when the ShowClipboard command is executed.
        /// </summary>
        private async Task OnShowClipboardExecuteAsync()
        {
            await _uiVisualizerService.ShowDialogAsync<ClipBoardViewModel>();
        }
    }
}
