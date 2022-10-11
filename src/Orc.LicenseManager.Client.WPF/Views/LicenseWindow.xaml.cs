namespace Orc.LicenseManager.Views
{
    using Catel;
    using Catel.Windows;
    using ViewModels;

    public partial class LicenseWindow : DataWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseWindow"/> class.
        /// </summary>
        public LicenseWindow()
            : this(null)
        {
        }

        /// <summary>5
        /// Initializes a new instance of the <see cref="LicenseWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public LicenseWindow(LicenseViewModel? viewModel)
            : base(viewModel, DataWindowMode.OkCancel)
        {
            InitializeComponent();

            if (CatelEnvironment.IsInDesignMode)
            {
                return;
            }

            LicenseManager.ResourceHelper.EnsureStyles();

            this.ApplyIconFromApplication();

            this.RemoveCloseButton();
        }
    }
}
