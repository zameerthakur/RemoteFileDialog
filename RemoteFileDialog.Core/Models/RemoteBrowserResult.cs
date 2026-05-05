namespace RemoteFileDialog.Core.Models;

/// <summary>
/// Represents the result of a remote browsing operation.
/// </summary>
public sealed class RemoteBrowserResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the selected folder path.
    /// </summary>
    public string? SelectedFolderPath { get; set; }

    /// <summary>
    /// Gets or sets the selected file paths.
    /// </summary>
    public IReadOnlyList<string> SelectedFilePaths { get; set; } = Array.Empty<string>();
}