using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RemoteFileDialog.Wpf.ViewModels;

/// <summary>
/// Represents a folder node in the remote folder tree.
/// </summary>
public sealed partial class RemoteTreeNodeViewModel : ObservableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteTreeNodeViewModel"/> class.
    /// </summary>
    /// <param name="name">The folder display name.</param>
    /// <param name="fullPath">The full remote folder path.</param>
    public RemoteTreeNodeViewModel(string name, string fullPath)
    {
        Name = name;
        FullPath = fullPath;
    }

    /// <summary>
    /// Gets the folder display name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the full remote folder path.
    /// </summary>
    public string FullPath { get; }

    /// <summary>
    /// Gets the child folder nodes.
    /// </summary>
    public ObservableCollection<RemoteTreeNodeViewModel> Children { get; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether the node has already loaded children.
    /// </summary>
    [ObservableProperty]
    private bool _isLoaded;

    /// <summary>
    /// Gets or sets a value indicating whether the node is expanded.
    /// </summary>
    [ObservableProperty]
    private bool _isExpanded;

    /// <summary>
    /// Gets or sets a value indicating whether the node is selected.
    /// </summary>
    [ObservableProperty]
    private bool _isSelected;
}