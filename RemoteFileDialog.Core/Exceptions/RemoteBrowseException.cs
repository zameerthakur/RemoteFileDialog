namespace RemoteFileDialog.Core.Exceptions;

/// <summary>
/// Represents an error that occurs while browsing or listing remote files and folders.
/// </summary>
public sealed class RemoteBrowseException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteBrowseException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public RemoteBrowseException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteBrowseException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public RemoteBrowseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}