namespace Orc.LicenseManager.Client.Example.ViewModels
{
    using Catel.MVVM;
    using Services;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ILicenseService _licenseService;

        #region Fields
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(ILicenseService licenseService)
            : base()
        {
            _licenseService = licenseService;

            ShowLicense = new Command(OnShowLicenseExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return "License manager example"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        #endregion

        #region Commands
        public Command ShowLicense { get; private set; }

        private void OnShowLicenseExecute()
        {
            _licenseService.ShowSingleLicenseDialog();
        }
        #endregion

        #region Methods

        #endregion
    }
}
