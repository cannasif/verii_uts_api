namespace uts_api.Application.Common.Models;

public class ApiResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();

    public static ApiResponse Ok(string message) => new()
    {
        Success = true,
        Message = message
    };

    public static ApiResponse Fail(string message, IReadOnlyCollection<string>? errors = null) => new()
    {
        Success = false,
        Message = message,
        Errors = errors ?? Array.Empty<string>()
    };
}
