using RemoteFileDialog.Core.Enums;
using RemoteFileDialog.Core.Events;
using RemoteFileDialog.Core.Interfaces;

namespace RemoteFileDialog.Infrastructure.Monitoring;

/// <summary>
/// Monitors a remote connection in the background while a dialog is open.
/// </summary>
public sealed class RemoteConnectionMonitor : IRemoteConnectionMonitor
{
    private readonly IRemoteBrowserService _remoteBrowserService;
    private readonly TimeSpan _checkInterval;
    private CancellationTokenSource? _monitorCancellationTokenSource;
    private Task? _monitorTask;
    private RemoteConnectionStatus? _lastStatus;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteConnectionMonitor"/> class.
    /// </summary>
    /// <param name="remoteBrowserService">The remote browser service to monitor.</param>
    /// <param name="checkInterval">The interval between connection checks.</param>
    public RemoteConnectionMonitor(
        IRemoteBrowserService remoteBrowserService,
        TimeSpan? checkInterval = null)
    {
        _remoteBrowserService = remoteBrowserService
            ?? throw new ArgumentNullException(nameof(remoteBrowserService));

        _checkInterval = checkInterval ?? TimeSpan.FromSeconds(10);
    }

    /// <summary>
    /// Occurs when the remote connection status changes.
    /// </summary>
    public event EventHandler<RemoteConnectionStatusChangedEventArgs>? StatusChanged;

    /// <summary>
    /// Gets a value indicating whether monitoring is currently running.
    /// </summary>
    public bool IsRunning { get; private set; }

    /// <summary>
    /// Starts background connection monitoring.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        if (IsRunning)
        {
            return Task.CompletedTask;
        }

        _monitorCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _monitorTask = MonitorAsync(_monitorCancellationTokenSource.Token);
        IsRunning = true;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops background connection monitoring.
    /// </summary>
    public async Task StopAsync()
    {
        if (!IsRunning)
        {
            return;
        }

        try
        {
            _monitorCancellationTokenSource?.Cancel();

            if (_monitorTask != null)
            {
                await _monitorTask;
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when monitoring is stopped.
        }
        finally
        {
            _monitorCancellationTokenSource?.Dispose();
            _monitorCancellationTokenSource = null;
            _monitorTask = null;
            IsRunning = false;
            _lastStatus = null;
        }
    }

    /// <summary>
    /// Releases monitor resources.
    /// </summary>
    public void Dispose()
    {
        _monitorCancellationTokenSource?.Cancel();
        _monitorCancellationTokenSource?.Dispose();
        _monitorCancellationTokenSource = null;
    }

    /// <summary>
    /// Runs the background monitoring loop.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel monitoring.</param>
    private async Task MonitorAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await _remoteBrowserService.IsConnectedAsync(cancellationToken);

            var status = result.IsSuccess && result.Data
                ? RemoteConnectionStatus.Connected
                : RemoteConnectionStatus.Disconnected;

            var message = status == RemoteConnectionStatus.Connected
                ? "Connected."
                : result.Message;

            RaiseStatusChangedIfNeeded(status, message);

            await Task.Delay(_checkInterval, cancellationToken);
        }
    }

    /// <summary>
    /// Raises the status changed event only when the status has changed.
    /// </summary>
    /// <param name="status">The current connection status.</param>
    /// <param name="message">The status message.</param>
    private void RaiseStatusChangedIfNeeded(
        RemoteConnectionStatus status,
        string message)
    {
        if (_lastStatus == status)
        {
            return;
        }

        _lastStatus = status;

        StatusChanged?.Invoke(
            this,
            new RemoteConnectionStatusChangedEventArgs(status, message));
    }
}