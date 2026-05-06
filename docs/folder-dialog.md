# Remote Folder Dialog

## Overview

`RemoteFolderDialog` provides a reusable WPF dialog for browsing and selecting remote folders from FTP and SFTP servers.

Features include:

- TreeView folder navigation
- Remote folder listing
- Connection monitoring
- Reconnect support
- Loading indicator
- Current path tracking

---

## Basic Usage

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

## Dialog Options

```csharp
var dialogOptions = new RemoteDialogOptions
{
    Title = "Select Remote Folder"
};

var dialog = new RemoteFolderDialog(
    connection,
    dialogOptions);
```

---

## Selected Folder Path

After the dialog closes successfully:

```csharp
var selectedFolder = dialog.SelectedFolderPath;
```

Example:

```text
/home/documents
```

---

## Connection Status

The dialog automatically displays:

- Connected status
- Disconnected status
- Reconnecting status
- Loading state

---

## Reconnect Handling

If the remote connection is lost while browsing:

- The dialog automatically updates status
- Folder and file views are cleared
- Reconnect button becomes available

After reconnecting:

- Last selected path is restored when possible

---

## Supported Servers

- FTP
- Explicit FTPS
- Implicit FTPS
- SFTP
