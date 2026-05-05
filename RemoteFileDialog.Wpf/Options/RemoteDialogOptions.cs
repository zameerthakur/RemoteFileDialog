namespace RemoteFileDialog.Wpf.Options;

/// <summary>
/// Represents configurable options for remote file and folder dialogs.
/// </summary>
public sealed class RemoteDialogOptions
{
    /// <summary>
    /// Gets or sets the dialog title.
    /// </summary>
    public string Title { get; set; } = "Remote Browser";

    /// <summary>
    /// Gets or sets a value indicating whether multiple file selection is allowed.
    /// </summary>
    public bool AllowMultipleSelection { get; set; } = true;

    /// <summary>
    /// Gets or sets the connection check interval while the dialog is open.
    /// </summary>
    public TimeSpan ConnectionCheckInterval { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Gets or sets a value indicating whether hidden files and folders should be shown.
    /// </summary>
    public bool ShowHiddenItems { get; set; }
}