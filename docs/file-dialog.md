# Remote File Dialog

## Overview

`RemoteFilePickerDialog` provides a reusable WPF dialog for browsing and selecting remote files from FTP and SFTP servers.

Features include:

- Remote file browsing
- Multi-file selection
- Header checkbox for select all files
- TreeView folder navigation
- Connection monitoring
- Loading indicator

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

---

## Single File Selection

```csharp
var dialog = new RemoteFilePickerDialog(
    connection,
    new RemoteDialogOptions
    {
        AllowMultipleSelection = false
    });
```

---

## Selected Files

After successful selection:

```csharp
var selectedFiles = dialog.SelectedFilePaths;
```

Example:

```text
/home/files/report.pdf
/home/files/data.csv
```

---

## Features

- Multi-file selection
- Select all / deselect all checkbox
- File size display
- Modified date display
- Reconnect support
- Current path tracking
