using RemoteFileDialog.Core.Enums;
using RemoteFileDialog.Core.Interfaces;
using RemoteFileDialog.Core.Models;
using RemoteFileDialog.Infrastructure.Clients;

namespace RemoteFileDialog.Infrastructure.Factory;

/// <summary>
/// Creates remote browser clients based on connection options.
/// </summary>
public sealed class RemoteBrowserClientFactory : IRemoteBrowserClientFactory
{
    /// <summary>
    /// Creates a remote browser client instance.
    /// </summary>
    public IRemoteBrowserClient Create(RemoteConnectionOptions options)
    {
        return options.ConnectionType switch
        {
            RemoteConnectionType.Ftp => new FtpRemoteBrowserClient(options),
            RemoteConnectionType.Sftp => new SftpRemoteBrowserClient(options),
            _ => throw new NotSupportedException("Unsupported connection type.")
        };
    }
}