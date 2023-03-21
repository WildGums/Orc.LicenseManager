namespace Orc.LicenseManager.ViewModels;

using System.Threading.Tasks;
using System.Windows;
using Catel.MVVM;

public class ClipBoardViewModel : ViewModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClipBoardViewModel"/> class.
    /// </summary>
    public ClipBoardViewModel()
    {
        Exit = new TaskCommand(OnExitExecuteAsync);

        var clipBoardData = Clipboard.GetText();
        ClipBoardText = string.IsNullOrWhiteSpace(clipBoardData) 
            ? "There is no text on your clipboard to display!"
            : clipBoardData;
    }

    public override string Title => "Clipboard";

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
    public TaskCommand Exit { get; }

    /// <summary>
    /// Method to invoke when the Exit command is executed.
    /// </summary>
    private async Task OnExitExecuteAsync()
    {
        await this.CancelAndCloseViewModelAsync();
    }
}
