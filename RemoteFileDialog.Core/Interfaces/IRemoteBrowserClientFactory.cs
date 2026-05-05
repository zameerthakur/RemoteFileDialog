using RemoteFileDialog.Core.Models;

namespace RemoteFileDialog.Core.Interfaces;

/// <summary>
/// Provides a factory for creating remote browser clients.
/// </summary>
public interface IRemoteBrowserClientFactory
{
    /// <summary>
    /// Creates a remote browser client based on connection options.
    /// </summary>
    IRemoteBrowserClient Create(RemoteConnectionOptions options);
}