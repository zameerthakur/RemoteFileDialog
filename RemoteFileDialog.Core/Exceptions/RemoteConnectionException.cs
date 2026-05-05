namespace RemoteFileDialog.Core.Exceptions;

/// <summary>
/// Represents an error that occurs while connecting, disconnecting, or checking a remote connection.
/// </summary>
public sealed class RemoteConnectionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteConnectionException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public RemoteConnectionException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteConnectionException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public RemoteConnectionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}