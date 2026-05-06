# Getting Started

## Requirements

- .NET 8 or later
- Windows with WPF support
- Visual Studio 2022 recommended

## Install Required Packages

```powershell
Install-Package FluentFTP
Install-Package SSH.NET
Install-Package CommunityToolkit.Mvvm
```

## Add Project Reference

Reference the following project:

```text
RemoteFileDialog.Wpf
```

## Supported Connection Types

### FTP

- FTP
- Explicit FTPS
- Implicit FTPS

### SFTP

- Password authentication
- Private key authentication
- Password + private key authentication

## Basic FTP Folder Dialog

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

## Basic File Dialog

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

## Features

- TreeView folder navigation
- Multi-file selection
- Connection status monitoring
- Reconnect support
- Loading indicators
- Header checkbox for select all files

## Sample Application

A working sample project is included:

```text
RemoteFileDialog.SampleApp
```
