using RemoteFileDialog.Core.Enums;
using RemoteFileDialog.Core.Interfaces;
using RemoteFileDialog.Core.Models;
using RemoteFileDialog.Core.Results;

namespace RemoteFileDialog.Infrastructure.Services;

/// <summary>
/// Provides high-level remote browsing operations with status and result handling.
/// </summary>
public sealed class RemoteBrowserService : IRemoteBrowserService
{
    private readonly RemoteConnectionOptions _options;
    private readonly IRemoteBrowserClientFactory _clientFactory;
    private IRemoteBrowserClient? _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteBrowserService"/> class.
    /// </summary>
    /// <param name="options">The remote connection options.</param>
    /// <param name="clientFactory">The remote browser client factory.</param>
    public RemoteBrowserService(
        RemoteConnectionOptions options,
        IRemoteBrowserClientFactory clientFactory)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

        Status = RemoteConnectionStatus.NotConnected;
        StatusMessage = "Not connected.";
    }

    /// <summary>
    /// Gets the current remote connection status.
    /// </summary>
    public RemoteConnectionStatus Status { get; private set; }

    /// <summary>
    /// Gets the current status message.
    /// </summary>
    public string StatusMessage { get; private set; }

    /// <summary>
    /// Connects to the remote server.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The operation result.</returns>
    public async Task<RemoteOperationResult> ConnectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            SetStatus(RemoteConnectionStatus.Connecting, "Connecting...");

            _client?.Dispose();
            _client = _clientFactory.Create(_options);

            await _client.ConnectAsync(cancellationToken);

            SetStatus(RemoteConnectionStatus.Connected, "Connected.");

            return RemoteOperationResult.Success("Connected.");
        }
        catch (Exception ex)
        {
            SetStatus(RemoteConnectionStatus.Failed, $"Connection failed. {ex.Message}");

            return RemoteOperationResult.Failure(StatusMessage, ex);
        }
    }

    /// <summary>
    /// Disconnects from the remote server.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The operation result.</returns>
    public async Task<RemoteOperationResult> DisconnectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_client != null)
            {
                await _client.DisconnectAsync(cancellationToken);
            }

            SetStatus(RemoteConnectionStatus.Disconnected, "Disconnected.");

            return RemoteOperationResult.Success("Disconnected.");
        }
        catch (Exception ex)
        {
            SetStatus(RemoteConnectionStatus.Failed, $"Disconnect failed. {ex.Message}");

            return RemoteOperationResult.Failure(StatusMessage, ex);
        }
    }

    /// <summary>
    /// Reconnects to the remote server.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The operation result.</returns>
    public async Task<RemoteOperationResult> ReconnectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            SetStatus(RemoteConnectionStatus.Reconnecting, "Reconnecting...");

            if (_client != null)
            {
                await _client.DisconnectAsync(cancellationToken);
                _client.Dispose();
                _client = null;
            }

            _client = _clientFactory.Create(_options);
            await _client.ConnectAsync(cancellationToken);

            SetStatus(RemoteConnectionStatus.Connected, "Connected.");

            return RemoteOperationResult.Success("Reconnected.");
        }
        catch (Exception ex)
        {
            SetStatus(RemoteConnectionStatus.Failed, $"Reconnect failed. {ex.Message}");

            return RemoteOperationResult.Failure(StatusMessage, ex);
        }
    }

    /// <summary>
    /// Checks whether the remote client is connected.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The operation result containing connection state.</returns>
    public async Task<RemoteOperationResult<bool>> IsConnectedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var isConnected = _client != null
                && await _client.IsConnectedAsync(cancellationToken);

            if (!isConnected)
            {
                SetStatus(RemoteConnectionStatus.Disconnected, "Disconnected.");
            }

            return RemoteOperationResult<bool>.Success(isConnected, StatusMessage);
        }
        catch (Exception ex)
        {
            SetStatus(RemoteConnectionStatus.Disconnected, $"Connection check failed. {ex.Message}");

            return RemoteOperationResult<bool>.Failure(StatusMessage, ex);
        }
    }

    /// <summary>
    /// Lists remote items from the specified path.
    /// </summary>
    /// <param name="path">The remote path.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The operation result containing remote items.</returns>
    public async Task<RemoteOperationResult<IReadOnlyList<RemoteItem>>> ListAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (_client == null)
            {
                SetStatus(RemoteConnectionStatus.Disconnected, "Disconnected.");
                return RemoteOperationResult<IReadOnlyList<RemoteItem>>.Failure("Client is not connected.");
            }

            var items = await _client.ListAsync(path, cancellationToken);

            SetStatus(RemoteConnectionStatus.Connected, "Connected.");

            return RemoteOperationResult<IReadOnlyList<RemoteItem>>.Success(items, "Items loaded.");
        }
        catch (Exception ex)
        {
            SetStatus(RemoteConnectionStatus.Disconnected, $"Listing failed. {ex.Message}");

            return RemoteOperationResult<IReadOnlyList<RemoteItem>>.Failure(StatusMessage, ex);
        }
    }

    /// <summary>
    /// Releases remote browser service resources.
    /// </summary>
    public void Dispose()
    {
        _client?.Dispose();
        _client = null;
    }

    /// <summary>
    /// Updates the connection status and status message.
    /// </summary>
    /// <param name="status">The connection status.</param>
    /// <param name="message">The status message.</param>
    private void SetStatus(RemoteConnectionStatus status, string message)
    {
        Status = status;
        StatusMessage = message;
    }
}