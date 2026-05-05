using RemoteFileDialog.Core.Enums;

namespace RemoteFileDialog.Core.Models;

/// <summary>
/// Represents connection settings for FTP or SFTP.
/// </summary>
public sealed class RemoteConnectionOptions
{
    /// <summary>
    /// Gets or sets the connection type (FTP or SFTP).
    /// </summary>
    public RemoteConnectionType ConnectionType { get; set; }

    /// <summary>
    /// Gets or sets the display name of the connection.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the server host address.
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the server port.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets the username for authentication.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for authentication.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the SFTP authentication type.
    /// </summary>
    public SftpAuthType SftpAuthType { get; set; } = SftpAuthType.Password;

    /// <summary>
    /// Gets or sets the private key file path for SFTP authentication.
    /// </summary>
    public string? PrivateKeyPath { get; set; }

    /// <summary>
    /// Gets or sets the passphrase for the private key.
    /// </summary>
    public string? PrivateKeyPassphrase { get; set; }

    /// <summary>
    /// Gets or sets the initial path to load when browsing starts.
    /// </summary>
    public string InitialPath { get; set; } = "/";
}