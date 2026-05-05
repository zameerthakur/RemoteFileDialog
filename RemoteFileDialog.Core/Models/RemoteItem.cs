using RemoteFileDialog.Core.Enums;

namespace RemoteFileDialog.Core.Models;

/// <summary>
/// Represents a file or directory on a remote server.
/// </summary>
public sealed class RemoteItem
{
    /// <summary>
    /// Gets or sets the name of the item.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full path of the item.
    /// </summary>
    public string FullPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the item.
    /// </summary>
    public RemoteItemType ItemType { get; set; }

    /// <summary>
    /// Gets a value indicating whether the item is a directory.
    /// </summary>
    public bool IsDirectory => ItemType == RemoteItemType.Directory;

    /// <summary>
    /// Gets a value indicating whether the item is a file.
    /// </summary>
    public bool IsFile => ItemType == RemoteItemType.File;

    /// <summary>
    /// Gets or sets the size of the file (if applicable).
    /// </summary>
    public long? Size { get; set; }

    /// <summary>
    /// Gets or sets the last modified date.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }
}