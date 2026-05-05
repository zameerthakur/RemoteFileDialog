using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RemoteFileDialog.Wpf.ViewModels;

/// <summary>
/// Provides ViewModel behavior for selecting one or more remote files.
/// </summary>
public sealed partial class RemoteFileDialogViewModel : ObservableObject
{
    private readonly RemoteBrowserViewModel _browser;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteFileDialogViewModel"/> class.
    /// </summary>
    /// <param name="browser">The shared remote browser ViewModel.</param>
    public RemoteFileDialogViewModel(RemoteBrowserViewModel browser)
    {
        _browser = browser ?? throw new ArgumentNullException(nameof(browser));
    }

    /// <summary>
    /// Gets the shared remote browser ViewModel.
    /// </summary>
    public RemoteBrowserViewModel Browser => _browser;

    /// <summary>
    /// Gets the selected remote file paths.
    /// </summary>
    public ObservableCollection<string> SelectedFilePaths { get; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether multiple file selection is allowed.
    /// </summary>
    [ObservableProperty]
    private bool _allowMultipleSelection = true;

    /// <summary>
    /// Updates selected files after a file checkbox is changed.
    /// </summary>
    /// <param name="item">The selected remote list item.</param>
    [RelayCommand]
    private void ToggleFileSelection(RemoteListItemViewModel? item)
    {
        if (item == null || !item.IsFile)
        {
            return;
        }

        if (!AllowMultipleSelection && item.IsSelected)
        {
            foreach (var listItem in Browser.Items.Where(x => x != item))
            {
                listItem.IsSelected = false;
            }
        }

        RefreshSelectedFiles();
        ConfirmSelectionCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Selects a single file and confirms it.
    /// </summary>
    /// <param name="item">The selected remote list item.</param>
    [RelayCommand]
    private void OpenFile(RemoteListItemViewModel? item)
    {
        if (item == null || !item.IsFile)
        {
            return;
        }

        if (!AllowMultipleSelection)
        {
            ClearAllSelections();
            item.IsSelected = true;
            RefreshSelectedFiles();
        }
    }

    /// <summary>
    /// Confirms the file selection.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanConfirmSelection))]
    private void ConfirmSelection()
    {
        // Dialog close will be handled by the View later.
    }

    /// <summary>
    /// Determines whether file selection can be confirmed.
    /// </summary>
    /// <returns>True if at least one file is selected; otherwise, false.</returns>
    private bool CanConfirmSelection()
    {
        return SelectedFilePaths.Count > 0;
    }

    /// <summary>
    /// Clears all selected files.
    /// </summary>
    private void ClearAllSelections()
    {
        foreach (var item in Browser.Items)
        {
            item.IsSelected = false;
        }

        SelectedFilePaths.Clear();
    }

    /// <summary>
    /// Refreshes the selected file path collection from selected items.
    /// </summary>
    private void RefreshSelectedFiles()
    {
        SelectedFilePaths.Clear();

        foreach (var item in Browser.Items.Where(x => x.IsFile && x.IsSelected))
        {
            SelectedFilePaths.Add(item.FullPath);
        }
    }
}