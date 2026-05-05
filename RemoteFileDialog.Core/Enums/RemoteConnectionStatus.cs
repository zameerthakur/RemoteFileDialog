namespace RemoteFileDialog.Core.Enums;

/// <summary>
/// Represents the current status of a remote connection.
/// </summary>
public enum RemoteConnectionStatus
{
    /// <summary>
    /// Not connected.
    /// </summary>
    NotConnected = 0,

    /// <summary>
    /// Connection is in progress.
    /// </summary>
    Connecting = 1,

    /// <summary>
    /// Successfully connected.
    /// </summary>
    Connected = 2,

    /// <summary>
    /// Connection is lost or unavailable.
    /// </summary>
    Disconnected = 3,

    /// <summary>
    /// Reconnection is in progress.
    /// </summary>
    Reconnecting = 4,

    /// <summary>
    /// Connection failed.
    /// </summary>
    Failed = 5
}