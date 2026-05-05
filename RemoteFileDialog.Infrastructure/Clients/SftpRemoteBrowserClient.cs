using Renci.SshNet;
using Renci.SshNet.Sftp;
using RemoteFileDialog.Core.Enums;
using RemoteFileDialog.Core.Interfaces;
using RemoteFileDialog.Core.Models;

namespace RemoteFileDialog.Infrastructure.Clients;

/// <summary>
/// Provides SFTP-based remote file system browsing.
/// </summary>
public sealed class SftpRemoteBrowserClient : IRemoteBrowserClient
{
    private readonly RemoteConnectionOptions _options;
    private SftpClient? _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="SftpRemoteBrowserClient"/> class.
    /// </summary>
    /// <param name="options">The SFTP connection options.</param>
    public SftpRemoteBrowserClient(RemoteConnectionOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Connects to the SFTP server.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var connectionInfo = CreateConnectionInfo();

            _client = new SftpClient(connectionInfo);

            await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                _client.Connect();
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"SFTP connection failed for host '{_options.Host}'. {ex.Message}",
                ex);
        }
    }

    /// <summary>
    /// Disconnects from the SFTP server.
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
            await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (_client.IsConnected)
                {
                    _client.Disconnect();
                }
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"SFTP disconnect failed for host '{_options.Host}'. {ex.Message}",
                ex);
        }
    }

    /// <summary>
    /// Determines whether the SFTP client is connected.
    /// </summary>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>True if connected; otherwise, false.</returns>
    public Task<bool> IsConnectedAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_client?.IsConnected == true);
    }

    /// <summary>
    /// Lists files and folders from the specified SFTP path.
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
            throw new InvalidOperationException("SFTP client is not connected.");
        }

        try
        {
            var listing = await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return _client.ListDirectory(path).ToList();
            }, cancellationToken);

            return listing
                .Where(item => item.Name != "." && item.Name != "..")
                .Where(item => item.IsRegularFile || item.IsDirectory)
                .Select(item => new RemoteItem
                {
                    Name = item.Name,
                    FullPath = NormalizeSftpPath(item.FullName),
                    ItemType = item.IsDirectory
                        ? RemoteItemType.Directory
                        : RemoteItemType.File,
                    Size = item.IsRegularFile ? item.Length : null,
                    ModifiedDate = item.LastWriteTime
                })
                .ToList();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"SFTP listing failed for path '{path}'. {ex.Message}",
                ex);
        }
    }

    /// <summary>
    /// Releases SFTP client resources.
    /// </summary>
    public void Dispose()
    {
        _client?.Dispose();
    }

    /// <summary>
    /// Creates SSH connection information based on configured SFTP authentication type.
    /// </summary>
    /// <returns>The SSH connection information.</returns>
    private ConnectionInfo CreateConnectionInfo()
    {
        ValidateBasicOptions();

        return _options.SftpAuthType switch
        {
            SftpAuthType.Password => CreatePasswordConnectionInfo(),
            SftpAuthType.PrivateKey => CreatePrivateKeyConnectionInfo(),
            SftpAuthType.PasswordAndPrivateKey => CreatePasswordAndPrivateKeyConnectionInfo(),
            _ => throw new NotSupportedException("Unsupported SFTP authentication type.")
        };
    }

    /// <summary>
    /// Creates connection information for username and password authentication.
    /// </summary>
    /// <returns>The SSH connection information.</returns>
    private ConnectionInfo CreatePasswordConnectionInfo()
    {
        if (string.IsNullOrWhiteSpace(_options.Password))
        {
            throw new InvalidOperationException("SFTP password is required.");
        }

        return new PasswordConnectionInfo(
            _options.Host,
            _options.Port,
            _options.Username,
            _options.Password);
    }

    /// <summary>
    /// Creates connection information for private key authentication.
    /// </summary>
    /// <returns>The SSH connection information.</returns>
    private ConnectionInfo CreatePrivateKeyConnectionInfo()
    {
        var privateKeyFile = CreatePrivateKeyFile();

        return new PrivateKeyConnectionInfo(
            _options.Host,
            _options.Port,
            _options.Username,
            privateKeyFile);
    }

    /// <summary>
    /// Creates connection information for password and private key authentication.
    /// </summary>
    /// <returns>The SSH connection information.</returns>
    private ConnectionInfo CreatePasswordAndPrivateKeyConnectionInfo()
    {
        if (string.IsNullOrWhiteSpace(_options.Password))
        {
            throw new InvalidOperationException("SFTP password is required.");
        }

        var privateKeyFile = CreatePrivateKeyFile();

        return new ConnectionInfo(
            _options.Host,
            _options.Port,
            _options.Username,
            new PasswordAuthenticationMethod(_options.Username, _options.Password),
            new PrivateKeyAuthenticationMethod(_options.Username, privateKeyFile));
    }

    /// <summary>
    /// Creates a private key file instance using configured key path and passphrase.
    /// </summary>
    /// <returns>The private key file.</returns>
    private PrivateKeyFile CreatePrivateKeyFile()
    {
        if (string.IsNullOrWhiteSpace(_options.PrivateKeyPath))
        {
            throw new InvalidOperationException("SFTP private key path is required.");
        }

        if (!File.Exists(_options.PrivateKeyPath))
        {
            throw new FileNotFoundException(
                "SFTP private key file was not found.",
                _options.PrivateKeyPath);
        }

        if (string.IsNullOrWhiteSpace(_options.PrivateKeyPassphrase))
        {
            return new PrivateKeyFile(_options.PrivateKeyPath);
        }

        return new PrivateKeyFile(
            _options.PrivateKeyPath,
            _options.PrivateKeyPassphrase);
    }

    /// <summary>
    /// Validates basic SFTP connection settings.
    /// </summary>
    private void ValidateBasicOptions()
    {
        if (string.IsNullOrWhiteSpace(_options.Host))
        {
            throw new InvalidOperationException("SFTP host is required.");
        }

        if (_options.Port <= 0)
        {
            throw new InvalidOperationException("SFTP port must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(_options.Username))
        {
            throw new InvalidOperationException("SFTP username is required.");
        }
    }

    /// <summary>
    /// Normalizes SFTP paths to use forward slashes.
    /// </summary>
    /// <param name="path">The path to normalize.</param>
    /// <returns>The normalized SFTP path.</returns>
    private static string NormalizeSftpPath(string path)
    {
        return string.IsNullOrWhiteSpace(path)
            ? "/"
            : path.Replace('\\', '/');
    }
}