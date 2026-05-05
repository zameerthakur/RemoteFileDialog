using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RemoteFileDialog.Core.Helpers;

namespace RemoteFileDialog.Wpf.ViewModels;

/// <summary>
/// Provides ViewModel behavior for selecting a remote folder.
/// </summary>
public sealed partial class RemoteFolderDialogViewModel : ObservableObject
{
    private readonly RemoteBrowserViewModel _browser;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteFolderDialogViewModel"/> class.
    /// </summary>
    /// <param name="browser">The shared remote browser ViewModel.</param>
    public RemoteFolderDialogViewModel(RemoteBrowserViewModel browser)
    {
        _browser = browser ?? throw new ArgumentNullException(nameof(browser));
    }

    /// <summary>
    /// Gets the shared remote browser ViewModel.
    /// </summary>
    public RemoteBrowserViewModel Browser => _browser;

    /// <summary>
    /// Gets or sets the selected folder path.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConfirmSelectionCommand))]
    private string? _selectedFolderPath;

    /// <summary>
    /// Selects a folder from the right-side folder list.
    /// </summary>
    /// <param name="item">The selected remote list item.</param>
    [RelayCommand]
    private void SelectFolder(RemoteListItemViewModel? item)
    {
        if (item == null || !item.IsDirectory)
        {
            return;
        }

        SelectedFolderPath = RemotePathHelper.Normalize(item.FullPath);
        Browser.LastSelectedPath = SelectedFolderPath;
    }

    /// <summary>
    /// Navigates into the selected folder.
    /// </summary>
    /// <param name="item">The selected remote list item.</param>
    [RelayCommand]
    private async Task OpenFolderAsync(RemoteListItemViewModel? item)
    {
        if (item == null || !item.IsDirectory)
        {
            return;
        }

        SelectedFolderPath = RemotePathHelper.Normalize(item.FullPath);
        Browser.LastSelectedPath = SelectedFolderPath;

        await Browser.SelectFolderAsync(SelectedFolderPath);
    }

    /// <summary>
    /// Confirms the folder selection.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanConfirmSelection))]
    private void ConfirmSelection()
    {
        // Dialog close will be handled by the View later.
    }

    /// <summary>
    /// Determines whether folder selection can be confirmed.
    /// </summary>
    /// <returns>True if a folder is selected; otherwise, false.</returns>
    private bool CanConfirmSelection()
    {
        return !string.IsNullOrWhiteSpace(SelectedFolderPath);
    }
}