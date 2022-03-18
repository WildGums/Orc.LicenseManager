[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName="")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orc/licensemanager", "Orc.LicenseManager")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orc/licensemanager", "Orc.LicenseManager.Converters")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orc/licensemanager", "Orc.LicenseManager.Views")]
[assembly: System.Windows.Markup.XmlnsPrefix("http://schemas.wildgums.com/orc/licensemanager", "orclicensemanager")]
public static class LoadAssembliesOnStartup { }
public static class ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.LicenseManager.Converters
{
    [System.Windows.Data.ValueConversion(typeof(object), typeof(string))]
    public class FormattingConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public FormattingConverter() { }
        protected FormattingConverter(string defaultFormatString) { }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
}
namespace Orc.LicenseManager
{
    public class DialogLicenseVisualizerService : Orc.LicenseManager.ILicenseVisualizerService
    {
        public DialogLicenseVisualizerService(Catel.Services.IUIVisualizerService uiVisualizerService, Orc.LicenseManager.ILicenseInfoService licenseInfoService, Catel.Services.IDispatcherService dispatcherService) { }
        public void ShowLicense() { }
    }
    public static class WindowExtensions
    {
        public static void ApplyIconFromApplication(this System.Windows.Window window) { }
        public static void RemoveCloseButton(this System.Windows.Window window) { }
    }
    public static class WpfNetworkValidationHelper
    {
        public static System.Threading.Tasks.Task DefaultNetworkLicenseServiceValidationHandlerAsync(object sender, Orc.LicenseManager.NetworkValidatedEventArgs e) { }
    }
}
namespace Orc.LicenseManager.ViewModels
{
    public class ClipBoardViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData ClipBoardTextProperty;
        public ClipBoardViewModel() { }
        public string ClipBoardText { get; set; }
        public Catel.MVVM.TaskCommand Exit { get; }
    }
    public class LicenseViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData AvailableLicenseModesProperty;
        public static readonly Catel.Data.PropertyData FailureMessageProperty;
        public static readonly Catel.Data.PropertyData FailureOccurredProperty;
        public static readonly Catel.Data.PropertyData ImageUriProperty;
        public static readonly Catel.Data.PropertyData InfoUrlProperty;
        public static readonly Catel.Data.PropertyData KeyProperty;
        public static readonly Catel.Data.PropertyData LicenseExistsProperty;
        public static readonly Catel.Data.PropertyData LicenseInfoProperty;
        public static readonly Catel.Data.PropertyData LicenseModeProperty;
        public static readonly Catel.Data.PropertyData PurchaseUrlProperty;
        public static readonly Catel.Data.PropertyData ShowFailureProperty;
        public static readonly Catel.Data.PropertyData TextProperty;
        public static readonly Catel.Data.PropertyData XmlDataProperty;
        public LicenseViewModel(Orc.LicenseManager.LicenseInfo licenseInfo, Catel.Services.INavigationService navigationService, Catel.Services.IProcessService processService, Orc.LicenseManager.ILicenseService licenseService, Orc.LicenseManager.ILicenseValidationService licenseValidationService, Catel.Services.IUIVisualizerService uiVisualizerService, Catel.Services.IMessageService messageService, Catel.Services.ILanguageService languageService, Orc.LicenseManager.ILicenseModeService licenseModeService) { }
        public System.Uri AboutImageUri { get; }
        public Catel.MVVM.Command AboutSiteClick { get; }
        public System.Collections.Generic.List<Orc.LicenseManager.LicenseMode> AvailableLicenseModes { get; }
        public string FailureMessage { get; }
        public bool FailureOccurred { get; set; }
        [Catel.MVVM.ViewModelToModel("LicenseInfo", "ImageUri")]
        public string ImageUri { get; set; }
        [Catel.MVVM.ViewModelToModel("LicenseInfo", "InfoUrl")]
        public string InfoUrl { get; set; }
        [Catel.MVVM.ViewModelToModel("LicenseInfo", "Key")]
        public string Key { get; set; }
        public bool LicenseExists { get; }
        public Orc.LicenseManager.LicenseMode LicenseMode { get; set; }
        public Catel.MVVM.TaskCommand Paste { get; }
        public Catel.MVVM.Command PurchaseLinkClick { get; }
        [Catel.MVVM.ViewModelToModel("LicenseInfo", "PurchaseUrl")]
        public string PurchaseUrl { get; set; }
        public Catel.MVVM.TaskCommand RemoveLicense { get; }
        public Catel.MVVM.TaskCommand ShowClipboard { get; }
        public bool ShowFailure { get; set; }
        [Catel.MVVM.ViewModelToModel("LicenseInfo", "Text")]
        public string Text { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.LicenseManager.XmlDataModel> XmlData { get; set; }
        protected override System.Threading.Tasks.Task<bool> CancelAsync() { }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
        protected override System.Threading.Tasks.Task<bool> SaveAsync() { }
        public string get_ImageUri() { }
        public string get_InfoUrl() { }
        public string get_Key() { }
        public string get_PurchaseUrl() { }
        public string get_Text() { }
        public void set_ImageUri(string ) { }
        public void set_InfoUrl(string ) { }
        public void set_Key(string ) { }
        public void set_PurchaseUrl(string ) { }
        public void set_Text(string ) { }
    }
    public class NetworkLicenseUsageViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData CurrentUsersProperty;
        public static readonly Catel.Data.PropertyData MaximumNumberOfConcurrentUsagesProperty;
        public static readonly Catel.Data.PropertyData PurchaseUrlProperty;
        public NetworkLicenseUsageViewModel(Orc.LicenseManager.NetworkValidationResult networkValidationResult, Orc.LicenseManager.ILicenseInfoService licenseInfoService, Catel.Services.IProcessService processService, Orc.LicenseManager.INetworkLicenseService networkLicenseService, Catel.Services.IDispatcherService dispatcherService) { }
        public Catel.MVVM.Command BuyLicenses { get; }
        public Catel.MVVM.Command CloseApplication { get; }
        public System.Collections.Generic.List<Orc.LicenseManager.NetworkLicenseUsage> CurrentUsers { get; set; }
        public int MaximumNumberOfConcurrentUsages { get; set; }
        public string PurchaseUrl { get; set; }
        protected override System.Threading.Tasks.Task CloseAsync() { }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
}
namespace Orc.LicenseManager.Views
{
    public class ClipBoardWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public ClipBoardWindow() { }
        public ClipBoardWindow(Orc.LicenseManager.ViewModels.ClipBoardViewModel viewModel) { }
        public void InitializeComponent() { }
    }
    public sealed class LicenseView : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty ShowAboutProperty;
        public LicenseView() { }
        public bool ShowAbout { get; set; }
        public void InitializeComponent() { }
    }
    public class LicenseWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public LicenseWindow() { }
        public LicenseWindow(Orc.LicenseManager.ViewModels.LicenseViewModel viewModel) { }
        public void InitializeComponent() { }
    }
    public class NetworkLicenseUsageWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public NetworkLicenseUsageWindow() { }
        public NetworkLicenseUsageWindow(Orc.LicenseManager.ViewModels.NetworkLicenseUsageViewModel viewModel) { }
        public void InitializeComponent() { }
    }
}