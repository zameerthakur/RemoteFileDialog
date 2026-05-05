using FluentFTP;
using RemoteFileDialog.Core.Enums;
using RemoteFileDialog.Core.Interfaces;
using RemoteFileDialog.Core.Models;

namespace RemoteFileDialog.Infrastructure.Clients;

/// <summary>
/// Provides FTP-based remote file system browsing.
/// </summary>
public sealed class FtpRemoteBrowserClient : IRemoteBrowserClient
{
    private readonly RemoteConnectionOptions _options;
    private AsyncFtpClient? _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="FtpRemoteBrowserClient"/> class.
    /// </summary>
    /// <param name="options">The FTP connection options.</param>
    public FtpRemoteBrowserClient(RemoteConnectionOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Connects to the FTP server.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _client = new AsyncFtpClient(
                _options.Host,
                _options.Username,
                _options.Password ?? string.Empty,
                _options.Port);

            await _client.Connect(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"FTP connection failed for host '{_options.Host}'. {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Disconnects from the FTP server.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (_client == null)
        {
            return;
        }

        try
        {
            if (_client.IsConnected)
            {
                await _client.Disconnect(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"FTP disconnect failed for host '{_options.Host}'. {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Determines whether the FTP client is connected.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>True if connected; otherwise, false.</returns>
    public Task<bool> IsConnectedAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_client?.IsConnected == true);
    }

    /// <summary>
    /// Lists files and folders from the specified FTP path.
    /// </summary>
    /// <param name="path">The remote path to list.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>A list of remote files and folders.</returns>
    public async Task<IReadOnlyList<RemoteItem>> ListAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        if (_client == null || !_client.IsConnected)
        {
            throw new InvalidOperationException("FTP client is not connected.");
        }

        try
        {
            var listing = await _client.GetListing(path, cancellationToken);

            return listing
                .Where(item => item.Type is FtpObjectType.File or FtpObjectType.Directory)
                .Select(item => new RemoteItem
                {
                    Name = item.Name,
                    FullPath = item.FullName,
                    ItemType = item.Type == FtpObjectType.Directory
                        ? RemoteItemType.Directory
                        : RemoteItemType.File,
                    Size = item.Type == FtpObjectType.File ? item.Size : null,
                    ModifiedDate = item.Modified
                })
                .ToList();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"FTP listing failed for path '{path}'. {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Releases FTP client resources.
    /// </summary>
    public void Dispose()
    {
        _client?.Dispose();
    }
}