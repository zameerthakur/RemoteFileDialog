namespace RemoteFileDialog.Core.Helpers;

/// <summary>
/// Provides helper methods for working with remote FTP and SFTP paths.
/// </summary>
public static class RemotePathHelper
{
    /// <summary>
    /// Normalizes a remote path to use forward slashes.
    /// </summary>
    /// <param name="path">The path to normalize.</param>
    /// <returns>The normalized remote path.</returns>
    public static string Normalize(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return "/";
        }

        var normalized = path.Replace('\\', '/').Trim();

        if (!normalized.StartsWith('/'))
        {
            normalized = "/" + normalized;
        }

        while (normalized.Contains("//"))
        {
            normalized = normalized.Replace("//", "/");
        }

        return normalized;
    }

    /// <summary>
    /// Combines a parent remote path with a child name.
    /// </summary>
    /// <param name="parentPath">The parent path.</param>
    /// <param name="childName">The child folder or file name.</param>
    /// <returns>The combined remote path.</returns>
    public static string Combine(string parentPath, string childName)
    {
        var parent = Normalize(parentPath);

        if (string.IsNullOrWhiteSpace(childName))
        {
            return parent;
        }

        childName = childName.Replace('\\', '/').Trim('/');

        return Normalize($"{parent}/{childName}");
    }

    /// <summary>
    /// Gets the parent path of the specified remote path.
    /// </summary>
    /// <param name="path">The remote path.</param>
    /// <returns>The parent remote path.</returns>
    public static string GetParentPath(string path)
    {
        var normalized = Normalize(path);

        if (normalized == "/")
        {
            return "/";
        }

        var lastSlashIndex = normalized.LastIndexOf('/');

        if (lastSlashIndex <= 0)
        {
            return "/";
        }

        return normalized[..lastSlashIndex];
    }

    /// <summary>
    /// Gets the name part from a remote path.
    /// </summary>
    /// <param name="path">The remote path.</param>
    /// <returns>The file or folder name.</returns>
    public static string GetName(string path)
    {
        var normalized = Normalize(path);

        if (normalized == "/")
        {
            return "/";
        }

        return normalized.Split('/', StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? "/";
    }
}