using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RemoteFileDialog.Core.Models;
using RemoteFileDialog.Wpf.Factories;
using RemoteFileDialog.Wpf.Options;
using RemoteFileDialog.Wpf.ViewModels;

namespace RemoteFileDialog.Wpf.Dialogs;

/// <summary>
/// Dialog used to browse and select one or more remote files.
/// </summary>
public partial class RemoteFilePickerDialog : Window
{
    private readonly RemoteFileDialogViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteFilePickerDialog"/> class.
    /// </summary>
    /// <param name="connectionOptions">The remote connection options.</param>
    /// <param name="dialogOptions">The dialog options.</param>
    public RemoteFilePickerDialog(
        RemoteConnectionOptions connectionOptions,
        RemoteDialogOptions? dialogOptions = null)
    {
        InitializeComponent();

        var factory = new RemoteBrowserViewModelFactory();

        _viewModel = factory.CreateFileDialogViewModel(
            connectionOptions,
            dialogOptions);

        DataContext = _viewModel;

        if (!string.IsNullOrWhiteSpace(dialogOptions?.Title))
        {
            Title = dialogOptions.Title;
        }
    }

    /// <summary>
    /// Gets the selected file paths.
    /// </summary>
    public IReadOnlyList<string> SelectedFilePaths => _viewModel.SelectedFilePaths.ToList();

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

        _viewModel.Browser.LastSelectedPath = node.FullPath;

        await _viewModel.Browser.SelectFolderAsync(node.FullPath);
    }

    /// <summary>
    /// Handles file double-click.
    /// </summary>
    private void FileList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ListView listView)
        {
            return;
        }

        if (listView.SelectedItem is RemoteListItemViewModel item)
        {
            _viewModel.OpenFileCommand.Execute(item);

            if (!_viewModel.AllowMultipleSelection && _viewModel.SelectedFilePaths.Count > 0)
            {
                DialogResult = true;
                Close();
            }
        }
    }

    /// <summary>
    /// Confirms selected files and closes the dialog.
    /// </summary>
    private void SelectButton_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.SelectedFilePaths.Count == 0)
        {
            return;
        }

        DialogResult = true;
        Close();
    }
}
