using CommunityToolkit.Mvvm.ComponentModel;
using RemoteFileDialog.Core.Enums;
using RemoteFileDialog.Core.Models;

namespace RemoteFileDialog.Wpf.ViewModels;

/// <summary>
/// Represents a remote item displayed in the right-side list.
/// </summary>
public sealed partial class RemoteListItemViewModel : ObservableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteListItemViewModel"/> class.
    /// </summary>
    /// <param name="remoteItem">The remote file or folder item.</param>
    public RemoteListItemViewModel(RemoteItem remoteItem)
    {
        RemoteItem = remoteItem ?? throw new ArgumentNullException(nameof(remoteItem));
    }

    /// <summary>
    /// Gets the underlying remote item.
    /// </summary>
    public RemoteItem RemoteItem { get; }

    /// <summary>
    /// Gets the item name.
    /// </summary>
    public string Name => RemoteItem.Name;

    /// <summary>
    /// Gets the full remote path.
    /// </summary>
    public string FullPath => RemoteItem.FullPath;

    /// <summary>
    /// Gets the item type.
    /// </summary>
    public RemoteItemType ItemType => RemoteItem.ItemType;

    /// <summary>
    /// Gets a value indicating whether this item is a directory.
    /// </summary>
    public bool IsDirectory => RemoteItem.IsDirectory;

    /// <summary>
    /// Gets a value indicating whether this item is a file.
    /// </summary>
    public bool IsFile => RemoteItem.IsFile;

    /// <summary>
    /// Gets the item size.
    /// </summary>
    public long? Size => RemoteItem.Size;

    /// <summary>
    /// Gets the item modified date.
    /// </summary>
    public DateTime? ModifiedDate => RemoteItem.ModifiedDate;

    /// <summary>
    /// Gets or sets a value indicating whether the item is selected.
    /// </summary>
    [ObservableProperty]
    private bool _isSelected;
}