using RemoteFileDialog.Core.Enums;
using RemoteFileDialog.Core.Models;
using RemoteFileDialog.Core.Results;

namespace RemoteFileDialog.Core.Interfaces;

/// <summary>
/// Defines high-level operations for browsing a remote file system.
/// </summary>
public interface IRemoteBrowserService : IDisposable
{
    /// <summary>
    /// Gets the current remote connection status.
    /// </summary>
    RemoteConnectionStatus Status { get; }

    /// <summary>
    /// Gets the current status message.
    /// </summary>
    string StatusMessage { get; }

    /// <summary>
    /// Connects to the remote server.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The operation result.</returns>
    Task<RemoteOperationResult> ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects from the remote server.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The operation result.</returns>
    Task<RemoteOperationResult> DisconnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reconnects to the remote server.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The operation result.</returns>
    Task<RemoteOperationResult> ReconnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether the remote client is connected.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The operation result containing connection state.</returns>
    Task<RemoteOperationResult<bool>> IsConnectedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists remote items from the specified path.
    /// </summary>
    /// <param name="path">The remote path.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The operation result containing remote items.</returns>
    Task<RemoteOperationResult<IReadOnlyList<RemoteItem>>> ListAsync(
        string path,
        CancellationToken cancellationToken = default);
}