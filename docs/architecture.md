# Architecture

RemoteFileDialog is organized into four projects.

## RemoteFileDialog.Core

Contains:

- Models
- Enums
- Interfaces
- Helpers
- Shared result objects

This project does not contain FTP, SFTP, or WPF implementation logic.

---

## RemoteFileDialog.Infrastructure

Contains:

- FTP client implementation
- SFTP client implementation
- Connection monitoring
- Remote browsing services

Third-party libraries are used here.

---

## RemoteFileDialog.Wpf

Contains:

- WPF dialogs
- ViewModels
- Dialog factories
- UI logic

Provides reusable dialogs for folder and file selection.

---

## RemoteFileDialog.SampleApp

Contains:

- Demo WPF application
- Example usage
- Manual testing environment

Used for validating features during development.
