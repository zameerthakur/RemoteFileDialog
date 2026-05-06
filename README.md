# RemoteFileDialog

Reusable WPF dialog for browsing and selecting files and folders from FTP and SFTP servers.

## Overview

RemoteFileDialog is a .NET WPF component that provides reusable dialog windows for remote file and folder selection.

It is designed for desktop applications that need FTP or SFTP browsing without building a custom remote explorer from scratch.

## Features

- Browse FTP folders
- Browse SFTP folders
- Select a remote folder path
- Select one or multiple remote files
- Left-side remote folder tree
- Right-side folder or file listing
- Connection status indicator
- Background connection monitoring
- Reconnect support
- SFTP password authentication
- SFTP private key authentication
- SFTP password + private key authentication
- Clean reusable architecture

## Dialog Types

### Remote Folder Dialog

Used to browse and select a remote folder path.

### Remote File Dialog

Used to browse and select one or more remote files.

## Supported Protocols

- FTP
- SFTP

## Target Framework

Planned target:

- .NET 8
- WPF
- Windows

## Quick Examples

### FTP Folder Dialog

```csharp
var connection = new RemoteConnectionOptions
{
    ConnectionType = RemoteConnectionType.Ftp,
    Host = "ftp.example.com",
    Port = 21,
    Username = "username",
    Password = "password"
};

var dialog = new RemoteFolderDialog(connection);

if (dialog.ShowDialog() == true)
{
    var selectedFolder = dialog.SelectedFolderPath;
}
```

---

### SFTP Folder Dialog (Password Authentication)

```csharp
var connection = new RemoteConnectionOptions
{
    ConnectionType = RemoteConnectionType.Sftp,
    Host = "sftp.example.com",
    Port = 22,
    Username = "username",
    Password = "password"
};

var dialog = new RemoteFolderDialog(connection);

if (dialog.ShowDialog() == true)
{
    var selectedFolder = dialog.SelectedFolderPath;
}
```

---

### SFTP Folder Dialog (Private Key Authentication)

```csharp
var connection = new RemoteConnectionOptions
{
    ConnectionType = RemoteConnectionType.Sftp,
    Host = "sftp.example.com",
    Port = 22,
    Username = "username",
    PrivateKeyFilePath = @"C:\Keys\privatekey.ppk",
    PrivateKeyPassphrase = "optional-passphrase"
};

var dialog = new RemoteFolderDialog(connection);

if (dialog.ShowDialog() == true)
{
    var selectedFolder = dialog.SelectedFolderPath;
}
```

---

### Remote File Dialog (Multi-Select)

```csharp
var connection = new RemoteConnectionOptions
{
    ConnectionType = RemoteConnectionType.Sftp,
    Host = "sftp.example.com",
    Port = 22,
    Username = "username",
    Password = "password"
};

var dialog = new RemoteFilePickerDialog(
    connection,
    new RemoteDialogOptions
    {
        AllowMultipleSelection = true
    });

if (dialog.ShowDialog() == true)
{
    var selectedFiles = dialog.SelectedFilePaths;
}
```

## Screenshots

### Remote Folder Dialog

![Remote Folder Dialog](assets/screenshots/folder-dialog.png)

### Remote File Dialog

![Remote File Dialog](assets/screenshots/file-dialog.png)

## 🚧 Project Status

This project is currently in active development, and the core FTP/SFTP browsing functionality is implemented and usable.

### ✅ Completed

- FTP browsing support
- SFTP browsing support
- Password authentication
- Private key authentication
- Password and private key authentication
- Remote folder selection dialog
- Remote file selection dialog
- Multi-file selection support
- Connection monitoring with automatic status updates
- Reconnect handling
- TreeView navigation with path synchronization
- Header checkbox for select all / deselect all files
- Loading indicators and UI status feedback
- Professional vector-based icons
- Standardized date formatting
- Human-readable file size formatting
- Documentation and usage examples
- Sample WPF application for testing

### 🔜 Planned

- NuGet package release
- Advanced filtering and sorting
- Dark mode support
- Search support
- File type filtering
- Localization support
- Keyboard navigation improvements
- Additional UI/UX polish
- TreeView lazy loading improvements

## License

This project is licensed under the MIT License.

## Third-Party Libraries

This project uses the following open-source libraries:

- FluentFTP (MIT License)
- SSH.NET (MIT License)
