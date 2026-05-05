using RemoteFileDialog.Core.Enums;

namespace RemoteFileDialog.Core.Events;

/// <summary>
/// Provides data for remote connection status change events.
/// </summary>
public sealed class RemoteConnectionStatusChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteConnectionStatusChangedEventArgs"/> class.
    /// </summary>
    /// <param name="status">The current connection status.</param>
    /// <param name="message">The current status message.</param>
    public RemoteConnectionStatusChangedEventArgs(
        RemoteConnectionStatus status,
        string message)
    {
        Status = status;
        Message = message;
    }

    /// <summary>
    /// Gets the current connection status.
    /// </summary>
    public RemoteConnectionStatus Status { get; }

    /// <summary>
    /// Gets the current status message.
    /// </summary>
    public string Message { get; }
}