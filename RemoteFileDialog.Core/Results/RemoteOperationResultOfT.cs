namespace RemoteFileDialog.Core.Results;

/// <summary>
/// Represents the result of a remote operation that returns data.
/// </summary>
/// <typeparam name="T">The type of data returned by the operation.</typeparam>
public sealed class RemoteOperationResult<T>
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
    /// Gets the returned data.
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// Gets the exception that caused the operation to fail, if available.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Creates a successful operation result with data.
    /// </summary>
    /// <param name="data">The operation data.</param>
    /// <param name="message">The success message.</param>
    /// <returns>A successful operation result.</returns>
    public static RemoteOperationResult<T> Success(T data, string message)
    {
        return new RemoteOperationResult<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    /// <summary>
    /// Creates a failed operation result.
    /// </summary>
    /// <param name="message">The failure message.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <returns>A failed operation result.</returns>
    public static RemoteOperationResult<T> Failure(string message, Exception? exception = null)
    {
        return new RemoteOperationResult<T>
        {
            IsSuccess = false,
            Message = message,
            Exception = exception
        };
    }
}