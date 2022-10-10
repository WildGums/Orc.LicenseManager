namespace Orc.LicenseManager.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.MVVM;

    /// <summary>
    /// Clipboard viewmodel
    /// </summary>
    public class ClipBoardViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClipBoardViewModel"/> class.
        /// </summary>
        public ClipBoardViewModel()
        {
            Title = "Clipboard";

            Exit = new TaskCommand(OnExitExecuteAsync);

            var clipBoardData = Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(clipBoardData))
            {
                ClipBoardText = "There is no text on your clipboard to display!";
            }
            else
            {
                ClipBoardText = clipBoardData;
            }
        }

        /// <summary>
        /// Gets or sets the clip board text.
        /// </summary>
        /// <value>
        /// The clip board text.
        /// </value>
        public string ClipBoardText { get; set; }

        /// <summary>
        /// Gets the Exit command.
        /// </summary>
        public TaskCommand Exit { get; private set; }

        /// <summary>
        /// Method to invoke when the Exit command is executed.
        /// </summary>
        private async Task OnExitExecuteAsync()
        {
            await this.CancelAndCloseViewModelAsync();
        }
    }
}
