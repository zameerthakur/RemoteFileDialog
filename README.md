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

## Project Status

This project is in early development.

## License

This project is licensed under the MIT License.

## Third-Party Libraries

This project uses the following open-source libraries:

- FluentFTP (MIT License)
- SSH.NET (MIT License)
