using RemoteFileDialog.Core.Interfaces;
using RemoteFileDialog.Core.Models;
using RemoteFileDialog.Infrastructure.Factory;
using RemoteFileDialog.Infrastructure.Monitoring;
using RemoteFileDialog.Infrastructure.Services;
using RemoteFileDialog.Wpf.Options;
using RemoteFileDialog.Wpf.ViewModels;

namespace RemoteFileDialog.Wpf.Factories;

/// <summary>
/// Creates ViewModels required by remote file and folder dialogs.
/// </summary>
public sealed class RemoteBrowserViewModelFactory
{
    /// <summary>
    /// Creates a folder dialog ViewModel.
    /// </summary>
    /// <param name="connectionOptions">The remote connection options.</param>
    /// <param name="dialogOptions">The dialog options.</param>
    /// <returns>The folder dialog ViewModel.</returns>
    public RemoteFolderDialogViewModel CreateFolderDialogViewModel(
        RemoteConnectionOptions connectionOptions,
        RemoteDialogOptions? dialogOptions = null)
    {
        var browser = CreateBrowserViewModel(connectionOptions, dialogOptions);
        return new RemoteFolderDialogViewModel(browser);
    }

    /// <summary>
    /// Creates a file dialog ViewModel.
    /// </summary>
    /// <param name="connectionOptions">The remote connection options.</param>
    /// <param name="dialogOptions">The dialog options.</param>
    /// <returns>The file dialog ViewModel.</returns>
    public RemoteFileDialogViewModel CreateFileDialogViewModel(
        RemoteConnectionOptions connectionOptions,
        RemoteDialogOptions? dialogOptions = null)
    {
        dialogOptions ??= new RemoteDialogOptions();

        var browser = CreateBrowserViewModel(connectionOptions, dialogOptions);

        return new RemoteFileDialogViewModel(browser)
        {
            AllowMultipleSelection = dialogOptions.AllowMultipleSelection
        };
    }

    /// <summary>
    /// Creates the shared browser ViewModel.
    /// </summary>
    /// <param name="connectionOptions">The remote connection options.</param>
    /// <param name="dialogOptions">The dialog options.</param>
    /// <returns>The shared browser ViewModel.</returns>
    private static RemoteBrowserViewModel CreateBrowserViewModel(
        RemoteConnectionOptions connectionOptions,
        RemoteDialogOptions? dialogOptions)
    {
        dialogOptions ??= new RemoteDialogOptions();

        IRemoteBrowserClientFactory clientFactory = new RemoteBrowserClientFactory();
        IRemoteBrowserService browserService = new RemoteBrowserService(connectionOptions, clientFactory);
        IRemoteConnectionMonitor monitor = new RemoteConnectionMonitor(
            browserService,
            dialogOptions.ConnectionCheckInterval);

        return new RemoteBrowserViewModel(
            connectionOptions,
            browserService,
            monitor);
    }
}