using System.Windows;
using RemoteFileDialog.Core.Enums;
using RemoteFileDialog.Core.Models;
using RemoteFileDialog.Wpf.Dialogs;
using RemoteFileDialog.Wpf.Options;

namespace RemoteFileDialog.SampleApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private RemoteConnectionOptions GetConnection()
    {
        return new RemoteConnectionOptions
        {
            ConnectionType = RemoteConnectionType.Ftp,
            //Host = "ftp.dlptest.com",
            //Port = 21,
            //Username = "dlpuser",
            //Password = "rNrKYTX9g7z3RgJRmxWuGHbeu"
            Host = "192.168.65.239", // public test FTP
            Port = 21,
            Username = @"ad\4607",
            Password = "gans@123"
        };
    }

    private void OpenFolderDialog_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new RemoteFolderDialog(GetConnection());

        if (dialog.ShowDialog() == true)
        {
            MessageBox.Show($"Folder:\n{dialog.SelectedFolderPath}");
        }
    }

    private void OpenFileDialog_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new RemoteFilePickerDialog(
            GetConnection(),
            new RemoteDialogOptions
            {
                AllowMultipleSelection = true
            });

        if (dialog.ShowDialog() == true)
        {
            var files = string.Join("\n", dialog.SelectedFilePaths);
            MessageBox.Show($"Selected Files:\n{files}");
        }
    }
}