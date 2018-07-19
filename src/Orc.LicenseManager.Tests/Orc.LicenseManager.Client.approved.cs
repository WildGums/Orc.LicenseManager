[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.6", FrameworkDisplayName=".NET Framework 4.6")]
public class static LoadAssembliesOnStartup { }
public class static ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.LicenseManager
{
    public abstract class ExpirationBehaviorBase : Orc.LicenseManager.IExpirationBehavior
    {
        protected ExpirationBehaviorBase() { }
        public virtual bool IsExpired(Portable.Licensing.License license, System.DateTime expirationDateTime, System.DateTime validationDateTime) { }
        protected abstract bool IsNormalLicenseExpired(Portable.Licensing.License license, System.DateTime expirationDateTime, System.DateTime validationDateTime);
    }
    public interface IExpirationBehavior
    {
        bool IsExpired(Portable.Licensing.License license, System.DateTime expirationDateTime, System.DateTime validationDateTime);
    }
    public class static ILicenseServiceExtensions
    {
        public static bool AnyExistingLicense(this Orc.LicenseManager.Services.ILicenseService licenseService) { }
        public static System.Nullable<System.DateTime> GetCurrentLicenseExpirationDateTime(this Orc.LicenseManager.Services.ILicenseService licenseService) { }
        public static string LoadExistingLicense(this Orc.LicenseManager.Services.ILicenseService licenseService) { }
    }
    public class static LicenseElements
    {
        public const string Expiration = "Expiration";
        public const string IdentificationSeparator = "-";
        public const string MachineId = "MachineID";
        public const string Version = "Version";
    }
    public class static LicenseExtensions
    {
        public static int GetMaximumConcurrentLicenses(this Portable.Licensing.License license) { }
    }
    public enum LicenseMode
    {
        [Catel.ComponentModel.DisplayNameAttribute("CurrentUser")]
        CurrentUser = 0,
        [Catel.ComponentModel.DisplayNameAttribute("AllUsers")]
        MachineWide = 1,
    }
    public class static LicenseModeExtensions
    {
        public static string ToDescriptionText(this Orc.LicenseManager.LicenseMode licenseMode) { }
        public static Orc.LicenseManager.LicenseMode ToOpposite(this Orc.LicenseManager.LicenseMode licenseMode) { }
    }
    public class static NetworkValidationHelper
    {
        public static void DefaultNetworkLicenseServiceValidationHandler(object sender, Orc.LicenseManager.Services.NetworkValidatedEventArgs e) { }
    }
    public class static NetworkValidationResultExtensions
    {
        public static Orc.LicenseManager.Models.NetworkLicenseUsage GetLatestUser(this Orc.LicenseManager.Models.NetworkValidationResult validationResult) { }
        public static bool IsCurrentUserLatestUser(this Orc.LicenseManager.Models.NetworkValidationResult validationResult) { }
    }
    public class PreventUsageOfAnyVersionExpirationBehavior : Orc.LicenseManager.ExpirationBehaviorBase
    {
        public PreventUsageOfAnyVersionExpirationBehavior() { }
        protected override bool IsNormalLicenseExpired(Portable.Licensing.License license, System.DateTime expirationDateTime, System.DateTime validationDateTime) { }
    }
    public class PreventUsageOfLaterReleasedVersionsExpirationBehavior : Orc.LicenseManager.ExpirationBehaviorBase
    {
        public PreventUsageOfLaterReleasedVersionsExpirationBehavior() { }
        protected override bool IsNormalLicenseExpired(Portable.Licensing.License license, System.DateTime expirationDateTime, System.DateTime validationDateTime) { }
    }
}
namespace Orc.LicenseManager.Models
{
    public class LicenseInfo : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData ImageUriProperty;
        public static readonly Catel.Data.PropertyData InfoUrlProperty;
        public static readonly Catel.Data.PropertyData KeyProperty;
        public static readonly Catel.Data.PropertyData PurchaseUrlProperty;
        public static readonly Catel.Data.PropertyData TextProperty;
        public static readonly Catel.Data.PropertyData TitleProperty;
        public LicenseInfo() { }
        public string ImageUri { get; set; }
        public string InfoUrl { get; set; }
        public string Key { get; set; }
        public string PurchaseUrl { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
    }
    public class LicenseValidationResult
    {
        public LicenseValidationResult() { }
        public string AdditionalInfo { get; set; }
        public bool IsValid { get; set; }
    }
    public class NetworkLicenseUsage
    {
        public NetworkLicenseUsage(string computerId, string ip, string userName, string licenseSignature, System.DateTime startDateTime) { }
        public string ComputerId { get; }
        public string Ip { get; }
        public string LicenseSignature { get; }
        public System.DateTime StartDateTime { get; }
        public string UserName { get; }
        public static Orc.LicenseManager.Models.NetworkLicenseUsage Parse(string text) { }
        public string ToNetworkMessage() { }
        public override string ToString() { }
    }
    public class NetworkValidationResult
    {
        public NetworkValidationResult() { }
        public System.Collections.Generic.List<Orc.LicenseManager.Models.NetworkLicenseUsage> CurrentUsers { get; }
        public bool IsValid { get; }
        public int MaximumConcurrentUsers { get; set; }
        public override string ToString() { }
    }
    public class ServerLicenseValidation
    {
        public ServerLicenseValidation() { }
        public string License { get; set; }
        public string MachineId { get; set; }
        public string ProductName { get; set; }
        public string ProductVersion { get; set; }
    }
    public class XmlDataModel
    {
        public XmlDataModel() { }
        public XmlDataModel(string name, string value) { }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
namespace Orc.LicenseManager.Services
{
    public class ApplicationIdService : Orc.LicenseManager.Services.IApplicationIdService
    {
        public ApplicationIdService() { }
        public string ApplicationId { get; set; }
        public string CompanyName { get; set; }
        public string ProductName { get; set; }
    }
    public class EmptyLicenseVisualizerService : Orc.LicenseManager.Services.ILicenseVisualizerService
    {
        public EmptyLicenseVisualizerService() { }
        public void ShowLicense() { }
    }
    public interface IApplicationIdService
    {
        string ApplicationId { get; set; }
        string CompanyName { get; set; }
        string ProductName { get; set; }
    }
    public class IdentificationService : Orc.LicenseManager.Services.IIdentificationService
    {
        public IdentificationService(Orc.SystemInfo.ISystemIdentificationService systemIdentificationService) { }
        public virtual string GetMachineId() { }
    }
    public interface IIdentificationService
    {
        string GetMachineId();
    }
    public interface ILicenseInfoService
    {
        Orc.LicenseManager.Models.LicenseInfo GetLicenseInfo();
    }
    public interface ILicenseLocationService
    {
        string GetLicenseLocation(Orc.LicenseManager.LicenseMode licenseMode);
        string LoadLicense(Orc.LicenseManager.LicenseMode licenseMode);
    }
    public interface ILicenseModeService
    {
        System.Collections.Generic.List<Orc.LicenseManager.LicenseMode> GetAvailableLicenseModes();
        bool IsLicenseModeAvailable(Orc.LicenseManager.LicenseMode licenseMode);
    }
    public interface ILicenseService
    {
        Portable.Licensing.License CurrentLicense { get; }
        bool LicenseExists(Orc.LicenseManager.LicenseMode licenseMode = 0);
        string LoadLicense(Orc.LicenseManager.LicenseMode licenseMode = 0);
        System.Collections.Generic.List<Orc.LicenseManager.Models.XmlDataModel> LoadXmlFromLicense(string license);
        void RemoveLicense(Orc.LicenseManager.LicenseMode licenseMode = 0);
        void SaveLicense(string license, Orc.LicenseManager.LicenseMode licenseMode = 0);
    }
    public interface ILicenseValidationService
    {
        Catel.Data.IValidationContext ValidateLicense(string license);
        Orc.LicenseManager.Models.LicenseValidationResult ValidateLicenseOnServer(string license, string serverUrl, System.Reflection.Assembly assembly = null);
        Catel.Data.IValidationContext ValidateXml(string license);
    }
    public class static ILicenseValidationServiceExtensions { }
    public interface ILicenseVisualizerService
    {
        void ShowLicense();
    }
    public interface IMachineLicenseValidationService
    {
        int Threshold { get; set; }
        Catel.Data.IValidationContext Validate(string machineIdToValidate);
    }
    public interface INetworkLicenseService
    {
        string ComputerId { get; }
        System.TimeSpan PollingInterval { get; }
        System.TimeSpan SearchTimeout { get; set; }
        public event System.EventHandler<Orc.LicenseManager.Services.NetworkValidatedEventArgs> Validated;
        void Initialize(System.TimeSpan pollingInterval = null);
        Orc.LicenseManager.Models.NetworkValidationResult ValidateLicense();
    }
    public class static INetworkLicenseServiceExtensions { }
    public interface ISimpleLicenseService
    {
        bool Validate();
        bool ValidateOnServer(string serverUrl);
    }
    public class LicenseLocationService : Orc.LicenseManager.Services.ILicenseLocationService
    {
        public LicenseLocationService(Orc.LicenseManager.Services.IApplicationIdService applicationIdService, Orc.FileSystem.IFileService fileService) { }
        public virtual string GetLicenseLocation(Orc.LicenseManager.LicenseMode licenseMode) { }
        public string LoadLicense(Orc.LicenseManager.LicenseMode licenseMode) { }
    }
    public class LicenseModeService : Orc.LicenseManager.Services.ILicenseModeService
    {
        public LicenseModeService(Orc.FileSystem.IFileService fileService, Orc.LicenseManager.Services.ILicenseLocationService licenseLocationService) { }
        public System.Collections.Generic.List<Orc.LicenseManager.LicenseMode> GetAvailableLicenseModes() { }
        public bool IsLicenseModeAvailable(Orc.LicenseManager.LicenseMode licenseMode) { }
    }
    public class LicenseService : Orc.LicenseManager.Services.ILicenseService
    {
        public LicenseService(Orc.LicenseManager.Services.ILicenseLocationService licenseLocationService, Orc.FileSystem.IFileService fileService) { }
        public Portable.Licensing.License CurrentLicense { get; }
        public bool LicenseExists(Orc.LicenseManager.LicenseMode licenseMode = 0) { }
        public string LoadLicense(Orc.LicenseManager.LicenseMode licenseMode = 0) { }
        public System.Collections.Generic.List<Orc.LicenseManager.Models.XmlDataModel> LoadXmlFromLicense(string license) { }
        public void RemoveLicense(Orc.LicenseManager.LicenseMode licenseMode = 0) { }
        public void SaveLicense(string license, Orc.LicenseManager.LicenseMode licenseMode = 0) { }
    }
    public class LicenseValidationService : Orc.LicenseManager.Services.ILicenseValidationService
    {
        public LicenseValidationService(Orc.LicenseManager.Services.IApplicationIdService applicationIdService, Orc.LicenseManager.IExpirationBehavior expirationBehavior, Orc.LicenseManager.Services.IIdentificationService identificationService, Orc.LicenseManager.Services.IMachineLicenseValidationService machineLicenseValidationService) { }
        public Catel.Data.IValidationContext ValidateLicense(string license) { }
        public Orc.LicenseManager.Models.LicenseValidationResult ValidateLicenseOnServer(string license, string serverUrl, System.Reflection.Assembly assembly = null) { }
        public Catel.Data.IValidationContext ValidateXml(string license) { }
    }
    public class MachineLicenseValidationService : Orc.LicenseManager.Services.IMachineLicenseValidationService
    {
        public MachineLicenseValidationService(Orc.LicenseManager.Services.IIdentificationService identificationService) { }
        public int Threshold { get; set; }
        public Catel.Data.IValidationContext Validate(string machineIdToValidate) { }
    }
    public class NetworkLicenseService : Orc.LicenseManager.Services.INetworkLicenseService
    {
        public NetworkLicenseService(Orc.LicenseManager.Services.ILicenseService licenseService, Orc.LicenseManager.Services.IIdentificationService identificationService) { }
        public string ComputerId { get; }
        public System.TimeSpan PollingInterval { get; }
        public System.TimeSpan SearchTimeout { get; set; }
        public event System.EventHandler<Orc.LicenseManager.Services.NetworkValidatedEventArgs> Validated;
        public virtual void Initialize(System.TimeSpan pollingInterval = null) { }
        public virtual Orc.LicenseManager.Models.NetworkValidationResult ValidateLicense() { }
    }
    public class NetworkValidatedEventArgs : System.EventArgs
    {
        public NetworkValidatedEventArgs(Orc.LicenseManager.Models.NetworkValidationResult validationResult) { }
        public Orc.LicenseManager.Models.NetworkValidationResult ValidationResult { get; }
    }
    public class SimpleLicenseService : Orc.LicenseManager.Services.ISimpleLicenseService
    {
        public SimpleLicenseService(Orc.LicenseManager.Services.ILicenseService licenseService, Orc.LicenseManager.Services.ILicenseValidationService licenseValidationService, Orc.LicenseManager.Services.ILicenseVisualizerService licenseVisualizerService) { }
        public bool Validate() { }
        public bool ValidateOnServer(string serverUrl) { }
    }
}