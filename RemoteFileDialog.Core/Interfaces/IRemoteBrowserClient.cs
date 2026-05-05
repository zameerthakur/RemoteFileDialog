using RemoteFileDialog.Core.Models;

namespace RemoteFileDialog.Core.Interfaces;

/// <summary>
/// Defines methods for interacting with a remote file system.
/// </summary>
public interface IRemoteBrowserClient : IDisposable
{
    /// <summary>
    /// Establishes a connection to the remote server.
    /// </summary>
    Task ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects from the remote server.
    /// </summary>
    Task DisconnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether the client is currently connected.
    /// </summary>
    Task<bool> IsConnectedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves files and folders from the specified path.
    /// </summary>
    Task<IReadOnlyList<RemoteItem>> ListAsync(
        string path,
        CancellationToken cancellationToken = default);
}