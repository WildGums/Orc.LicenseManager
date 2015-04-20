namespace Orc.LicenseManager.Views
{
    using Catel.Windows;

    using ViewModels;

    /// <summary>
    /// Interaction logic for ClipBoardWindow.xaml.
    /// </summary>
    public partial class ClipBoardWindow : DataWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClipBoardWindow"/> class.
        /// </summary>
        public ClipBoardWindow()
        {
            InitializeComponent();
        }
                /// <summary>
        /// Initializes a new instance of the <see cref="LicenseWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public ClipBoardWindow(ClipBoardViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            InitializeComponent();

            this.ApplyIconFromApplication();
        }
    }
}
