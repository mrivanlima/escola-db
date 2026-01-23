# Prompt: Standardize API Response (Envelope Pattern)

**Context:**
We want to establish a strict JSON contract for all API responses. Currently, the `StudentsController` returns anonymous objects (e.g., `new { success = true... }`). We need a strongly-typed generic wrapper to ensure consistency for the Frontend.

**Goal:**
1. Create a generic `ApiResponse<T>` wrapper.
2. Refactor `StudentsController` to use this wrapper.

**The Prompt:**

@src/Escola.Api/Controllers/StudentsController.cs

Act as a Senior .NET Developer.
Refactor the API to use a Standard Response Envelope.

**Task 1: Create the Wrapper**
Create a new file `src/Escola.Api/Common/ApiResponse.cs`.
The class should look like this:
```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }

    public ApiResponse() { }

    public ApiResponse(T data, string message = null)
    {
        Success = true;
        Message = message;
        Data = data;
        Errors = null;
    }

    public ApiResponse(bool success, string message, List<string> errors = null)
    {
        Success = success;
        Message = message;
        Data = default;
        Errors = errors;
    }

    // Factory methods for cleaner code
    public static ApiResponse<T> SuccessResult(T data, string message = null) 
        => new ApiResponse<T>(data, message);

    public static ApiResponse<T> FailureResult(string message, List<string> errors = null) 
        => new ApiResponse<T>(false, message, errors);
}