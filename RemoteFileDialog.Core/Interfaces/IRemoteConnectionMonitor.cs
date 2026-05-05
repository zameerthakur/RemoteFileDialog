using RemoteFileDialog.Core.Events;

namespace RemoteFileDialog.Core.Interfaces;

/// <summary>
/// Defines background monitoring for a remote connection.
/// </summary>
public interface IRemoteConnectionMonitor : IDisposable
{
    /// <summary>
    /// Occurs when the remote connection status changes.
    /// </summary>
    event EventHandler<RemoteConnectionStatusChangedEventArgs>? StatusChanged;

    /// <summary>
    /// Gets a value indicating whether monitoring is currently running.
    /// </summary>
    bool IsRunning { get; }

    /// <summary>
    /// Starts background connection monitoring.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    Task StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops background connection monitoring.
    /// </summary>
    Task StopAsync();
}