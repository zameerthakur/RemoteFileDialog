using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RemoteFileDialog.Core.Enums;
using RemoteFileDialog.Core.Events;
using RemoteFileDialog.Core.Helpers;
using RemoteFileDialog.Core.Interfaces;
using RemoteFileDialog.Core.Models;

namespace RemoteFileDialog.Wpf.ViewModels;

/// <summary>
/// Provides common remote browsing behavior for folder and file dialogs.
/// </summary>
public sealed partial class RemoteBrowserViewModel : ObservableObject, IDisposable
{
    private readonly IRemoteBrowserService _browserService;
    private readonly IRemoteConnectionMonitor _connectionMonitor;
    private readonly RemoteConnectionOptions _connectionOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteBrowserViewModel"/> class.
    /// </summary>
    /// <param name="connectionOptions">The remote connection options.</param>
    /// <param name="browserService">The remote browser service.</param>
    /// <param name="connectionMonitor">The remote connection monitor.</param>
    public RemoteBrowserViewModel(
        RemoteConnectionOptions connectionOptions,
        IRemoteBrowserService browserService,
        IRemoteConnectionMonitor connectionMonitor)
    {
        _connectionOptions = connectionOptions ?? throw new ArgumentNullException(nameof(connectionOptions));
        _browserService = browserService ?? throw new ArgumentNullException(nameof(browserService));
        _connectionMonitor = connectionMonitor ?? throw new ArgumentNullException(nameof(connectionMonitor));

        Status = RemoteConnectionStatus.NotConnected;
        StatusMessage = "Not connected.";
        CurrentPath = RemotePathHelper.Normalize(_connectionOptions.InitialPath);

        _connectionMonitor.StatusChanged += OnConnectionStatusChanged;
    }

    /// <summary>
    /// Gets the folder tree nodes.
    /// </summary>
    public ObservableCollection<RemoteTreeNodeViewModel> TreeNodes { get; } = [];

    /// <summary>
    /// Gets the right-side remote item list.
    /// </summary>
    public ObservableCollection<RemoteListItemViewModel> Items { get; } = [];

    /// <summary>
    /// Gets the display name of the connection.
    /// </summary>
    public string ConnectionDisplayName =>
        string.IsNullOrWhiteSpace(_connectionOptions.DisplayName)
            ? $"{_connectionOptions.ConnectionType} - {_connectionOptions.Host}"
            : $"{_connectionOptions.ConnectionType} - {_connectionOptions.DisplayName}";

    /// <summary>
    /// Gets or sets the current remote path.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RefreshCommand))]
    private string _currentPath = "/";

    /// <summary>
    /// Gets or sets the last selected path.
    /// </summary>
    [ObservableProperty]
    private string? _lastSelectedPath;

    /// <summary>
    /// Gets or sets the last browsed path.
    /// </summary>
    [ObservableProperty]
    private string? _lastBrowsedPath;

    /// <summary>
    /// Gets or sets the current connection status.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ReconnectCommand))]
    [NotifyCanExecuteChangedFor(nameof(RefreshCommand))]
    private RemoteConnectionStatus _status;

    /// <summary>
    /// Gets or sets the current status message.
    /// </summary>
    [ObservableProperty]
    private string _statusMessage = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the browser is connected.
    /// </summary>
    public bool IsConnected => Status == RemoteConnectionStatus.Connected;

    /// <summary>
    /// Gets a value indicating whether reconnect is available.
    /// </summary>
    public bool CanReconnect =>
        Status is RemoteConnectionStatus.Disconnected or RemoteConnectionStatus.Failed;

    /// <summary>
    /// Initializes the remote browser.
    /// </summary>
    [RelayCommand]
    public async Task InitializeAsync()
    {
        var connectResult = await _browserService.ConnectAsync();

        Status = _browserService.Status;
        StatusMessage = connectResult.Message;

        if (!connectResult.IsSuccess)
        {
            ClearBrowser();
            return;
        }

        await LoadPathAsync(CurrentPath);
        await _connectionMonitor.StartAsync();
    }

    /// <summary>
    /// Refreshes the current remote path.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanRefresh))]
    public async Task RefreshAsync()
    {
        await LoadPathAsync(CurrentPath);
    }

    /// <summary>
    /// Reconnects to the remote server and restores the best available path.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanReconnect))]
    public async Task ReconnectAsync()
    {
        var reconnectResult = await _browserService.ReconnectAsync();

        Status = _browserService.Status;
        StatusMessage = reconnectResult.Message;

        if (!reconnectResult.IsSuccess)
        {
            ClearBrowser();
            return;
        }

        var restorePath = GetRestorePath();

        await LoadPathAsync(restorePath);
        await _connectionMonitor.StartAsync();
    }

    /// <summary>
    /// Loads folders and files from the specified remote path.
    /// </summary>
    /// <param name="path">The remote path to load.</param>
    public async Task LoadPathAsync(string path)
    {
        var normalizedPath = RemotePathHelper.Normalize(path);
        var result = await _browserService.ListAsync(normalizedPath);

        Status = _browserService.Status;
        StatusMessage = result.Message;

        if (!result.IsSuccess || result.Data == null)
        {
            ClearBrowser();
            return;
        }

        CurrentPath = normalizedPath;
        LastBrowsedPath = normalizedPath;

        Items.Clear();

        foreach (var item in result.Data.OrderByDescending(x => x.IsDirectory).ThenBy(x => x.Name))
        {
            Items.Add(new RemoteListItemViewModel(item));
        }

        EnsureRootNode();
    }

    /// <summary>
    /// Handles a folder selection from tree or list.
    /// </summary>
    /// <param name="folderPath">The selected folder path.</param>
    public async Task SelectFolderAsync(string folderPath)
    {
        var normalizedPath = RemotePathHelper.Normalize(folderPath);

        LastSelectedPath = normalizedPath;

        await LoadPathAsync(normalizedPath);
    }

    /// <summary>
    /// Clears all browser data after disconnection.
    /// </summary>
    public void ClearBrowser()
    {
        TreeNodes.Clear();
        Items.Clear();
        CurrentPath = "-";
    }

    /// <summary>
    /// Releases browser resources.
    /// </summary>
    public void Dispose()
    {
        _connectionMonitor.StatusChanged -= OnConnectionStatusChanged;
        _connectionMonitor.Dispose();
        _browserService.Dispose();
    }

    /// <summary>
    /// Determines whether refresh is available.
    /// </summary>
    /// <returns>True when refresh is allowed; otherwise, false.</returns>
    private bool CanRefresh()
    {
        return Status == RemoteConnectionStatus.Connected &&
               !string.IsNullOrWhiteSpace(CurrentPath) &&
               CurrentPath != "-";
    }

    /// <summary>
    /// Handles connection monitor status changes.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The status changed event arguments.</param>
    private void OnConnectionStatusChanged(
        object? sender,
        RemoteConnectionStatusChangedEventArgs e)
    {
        Status = e.Status;
        StatusMessage = e.Message;

        if (Status == RemoteConnectionStatus.Disconnected)
        {
            ClearBrowser();
        }
    }

    /// <summary>
    /// Gets the best path to restore after reconnecting.
    /// </summary>
    /// <returns>The restore path.</returns>
    private string GetRestorePath()
    {
        if (!string.IsNullOrWhiteSpace(LastSelectedPath))
        {
            return LastSelectedPath;
        }

        if (!string.IsNullOrWhiteSpace(LastBrowsedPath))
        {
            return LastBrowsedPath;
        }

        if (!string.IsNullOrWhiteSpace(_connectionOptions.InitialPath))
        {
            return _connectionOptions.InitialPath;
        }

        return "/";
    }

    /// <summary>
    /// Ensures the root tree node exists.
    /// </summary>
    private void EnsureRootNode()
    {
        if (TreeNodes.Count > 0)
        {
            return;
        }

        TreeNodes.Add(new RemoteTreeNodeViewModel("/", "/"));
    }

    /// <summary>
    /// Updates dependent calculated properties when status changes.
    /// </summary>
    /// <param name="value">The new connection status.</param>
    partial void OnStatusChanged(RemoteConnectionStatus value)
    {
        OnPropertyChanged(nameof(IsConnected));
        OnPropertyChanged(nameof(CanReconnect));
    }
}