namespace RemoteFileDialog.Core.Enums;

/// <summary>
/// Defines authentication types supported for SFTP connections.
/// </summary>
public enum SftpAuthType
{
    /// <summary>
    /// Username and password authentication.
    /// </summary>
    Password = 1,

    /// <summary>
    /// Private key authentication.
    /// </summary>
    PrivateKey = 2,

    /// <summary>
    /// Username, password, and private key authentication.
    /// </summary>
    PasswordAndPrivateKey = 3
}