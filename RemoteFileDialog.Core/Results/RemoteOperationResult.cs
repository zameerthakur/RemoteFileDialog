namespace RemoteFileDialog.Core.Results;

/// <summary>
/// Represents the result of a remote operation.
/// </summary>
public sealed class RemoteOperationResult
{
    /// <summary>
    /// Gets a value indicating whether the operation completed successfully.
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Gets the user-friendly message for the operation result.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Gets the exception that caused the operation to fail, if available.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Creates a successful operation result.
    /// </summary>
    /// <param name="message">The success message.</param>
    /// <returns>A successful operation result.</returns>
    public static RemoteOperationResult Success(string message)
    {
        return new RemoteOperationResult
        {
            IsSuccess = true,
            Message = message
        };
    }

    /// <summary>
    /// Creates a failed operation result.
    /// </summary>
    /// <param name="message">The failure message.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <returns>A failed operation result.</returns>
    public static RemoteOperationResult Failure(string message, Exception? exception = null)
    {
        return new RemoteOperationResult
        {
            IsSuccess = false,
            Message = message,
            Exception = exception
        };
    }
}