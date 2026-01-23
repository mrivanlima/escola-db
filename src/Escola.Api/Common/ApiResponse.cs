namespace Escola.Api.Common;

/// <summary>
/// Standard API response envelope for all endpoints.
/// Provides consistent structure for success/failure responses.
/// </summary>
/// <typeparam name="T">Type of data being returned</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates if the request was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Optional message describing the result.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// The actual data payload.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// List of error messages if the request failed.
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public ApiResponse() { }

    /// <summary>
    /// Success constructor.
    /// </summary>
    /// <param name="data">Data to return</param>
    /// <param name="message">Optional success message</param>
    public ApiResponse(T data, string? message = null)
    {
        Success = true;
        Message = message;
        Data = data;
        Errors = null;
    }

    /// <summary>
    /// Failure constructor.
    /// </summary>
    /// <param name="success">Success flag</param>
    /// <param name="message">Error message</param>
    /// <param name="errors">List of detailed errors</param>
    public ApiResponse(bool success, string message, List<string>? errors = null)
    {
        Success = success;
        Message = message;
        Data = default;
        Errors = errors;
    }

    /// <summary>
    /// Factory method for success responses.
    /// </summary>
    /// <param name="data">Data to return</param>
    /// <param name="message">Optional success message</param>
    /// <returns>Success response</returns>
    public static ApiResponse<T> SuccessResult(T data, string? message = null)
        => new ApiResponse<T>(data, message);

    /// <summary>
    /// Factory method for failure responses.
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="errors">List of detailed errors</param>
    /// <returns>Failure response</returns>
    public static ApiResponse<T> FailureResult(string message, List<string>? errors = null)
        => new ApiResponse<T>(false, message, errors);
}
