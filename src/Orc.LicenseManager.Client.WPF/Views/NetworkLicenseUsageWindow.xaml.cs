namespace Orc.LicenseManager.Views
{
    using Catel.Windows;
    using ViewModels;

    public partial class NetworkLicenseUsageWindow
    {
        public NetworkLicenseUsageWindow()
            : this(null)
        {
        }

        public NetworkLicenseUsageWindow(NetworkLicenseUsageViewModel? viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            InitializeComponent();
        }
    }
}
