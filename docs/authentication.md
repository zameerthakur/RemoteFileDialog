# Authentication

## Supported SFTP Authentication Types

RemoteFileDialog supports the following SFTP authentication methods:

- Password authentication
- Private key authentication
- Password and private key authentication

---

## Password Authentication

```csharp
var connection = new RemoteConnectionOptions
{
    ConnectionType = RemoteConnectionType.Sftp,
    Host = "sftp.example.com",
    Port = 22,
    Username = "username",
    Password = "password"
};
```

---

## Private Key Authentication

```csharp
var connection = new RemoteConnectionOptions
{
    ConnectionType = RemoteConnectionType.Sftp,
    Host = "sftp.example.com",
    Port = 22,
    Username = "username",
    SftpAuthType = SftpAuthType.PrivateKey,
    PrivateKeyPath = @"C:\Keys\private-key.ppk"
};
```

---

## Password and Private Key Authentication

```csharp
var connection = new RemoteConnectionOptions
{
    ConnectionType = RemoteConnectionType.Sftp,
    Host = "sftp.example.com",
    Port = 22,
    Username = "username",
    Password = "password",
    SftpAuthType = SftpAuthType.PasswordAndPrivateKey,
    PrivateKeyPath = @"C:\Keys\private-key.ppk",
    PrivateKeyPassphrase = "optional-passphrase"
};
```

---

## Supported Private Key Formats

- `.ppk`
- `.pem`
- OpenSSH private keys

---

## FTP and FTPS Support

RemoteFileDialog also supports:

- FTP
- Explicit FTPS
- Implicit FTPS

Example:

```csharp
var connection = new RemoteConnectionOptions
{
    ConnectionType = RemoteConnectionType.Ftp,
    Host = "ftp.example.com",
    Port = 21,
    Username = "username",
    Password = "password"
};
```
