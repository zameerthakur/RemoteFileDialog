using System.Windows;
using RemoteFileDialog.Core.Enums;
using RemoteFileDialog.Core.Models;
using RemoteFileDialog.Wpf.Dialogs;
using RemoteFileDialog.Wpf.Options;

namespace RemoteFileDialog.SampleApp;

/// <summary>
/// Main application window.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OpenDialog_Click(object sender, RoutedEventArgs e)
    {
        var connectionOptions = new RemoteConnectionOptions
        {
            ConnectionType = RemoteConnectionType.Ftp, // change to Sftp if needed
            Host = "192.168.65.239",                  // public test FTP
            Port = 21,
            Username = @"ad\4607",
            Password = "gans@123"
        };

        var dialogOptions = new RemoteDialogOptions
        {
            Title = "Select Remote Folder"
        };

        var dialog = new RemoteFolderDialog(connectionOptions, dialogOptions);

        if (dialog.ShowDialog() == true)
        {
            MessageBox.Show($"Selected Folder:\n{dialog.SelectedFolderPath}");
        }
    }
}