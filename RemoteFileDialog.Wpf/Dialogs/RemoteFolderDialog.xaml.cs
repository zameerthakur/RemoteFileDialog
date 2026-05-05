using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RemoteFileDialog.Core.Models;
using RemoteFileDialog.Wpf.Factories;
using RemoteFileDialog.Wpf.Options;
using RemoteFileDialog.Wpf.ViewModels;

namespace RemoteFileDialog.Wpf.Dialogs;

/// <summary>
/// Dialog used to browse and select a remote folder.
/// </summary>
public partial class RemoteFolderDialog : Window
{
    private readonly RemoteFolderDialogViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteFolderDialog"/> class.
    /// </summary>
    /// <param name="connectionOptions">The remote connection options.</param>
    /// <param name="dialogOptions">The dialog options.</param>
    public RemoteFolderDialog(
        RemoteConnectionOptions connectionOptions,
        RemoteDialogOptions? dialogOptions = null)
    {
        InitializeComponent();

        var factory = new RemoteBrowserViewModelFactory();

        _viewModel = factory.CreateFolderDialogViewModel(
            connectionOptions,
            dialogOptions);

        DataContext = _viewModel;

        if (!string.IsNullOrWhiteSpace(dialogOptions?.Title))
        {
            Title = dialogOptions.Title;
        }
    }

    /// <summary>
    /// Gets the selected folder path.
    /// </summary>
    public string? SelectedFolderPath => _viewModel.SelectedFolderPath;

    /// <summary>
    /// Initializes remote browsing when the dialog is loaded.
    /// </summary>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.Browser.InitializeAsync();
    }

    /// <summary>
    /// Releases ViewModel resources when the dialog closes.
    /// </summary>
    private void Window_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        _viewModel.Browser.Dispose();
    }

    /// <summary>
    /// Handles folder selection from the right-side folder list.
    /// </summary>
    private void FolderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListView listView)
        {
            return;
        }

        if (listView.SelectedItem is RemoteListItemViewModel item)
        {
            _viewModel.SelectFolderCommand.Execute(item);
        }
    }

    /// <summary>
    /// Opens the selected folder on double-click.
    /// </summary>
    private void FolderList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ListView listView)
        {
            return;
        }

        if (listView.SelectedItem is RemoteListItemViewModel item)
        {
            _viewModel.OpenFolderCommand.Execute(item);
        }
    }

    /// <summary>
    /// Confirms selected folder and closes the dialog.
    /// </summary>
    private void SelectButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_viewModel.SelectedFolderPath))
        {
            return;
        }

        DialogResult = true;
        Close();
    }

    /// <summary>
    /// Handles folder selection from the left-side folder tree.
    /// </summary>
    private async void FolderTree_SelectedItemChanged(
        object sender,
        RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is not RemoteTreeNodeViewModel node)
        {
            return;
        }

        _viewModel.SelectedFolderPath = node.FullPath;
        _viewModel.Browser.LastSelectedPath = node.FullPath;

        await _viewModel.Browser.SelectFolderAsync(node.FullPath);
    }
}