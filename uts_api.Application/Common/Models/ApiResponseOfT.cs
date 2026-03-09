namespace uts_api.Application.Common.Models;

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; init; }

    public static ApiResponse<T> Ok(T data, string message) => new()
    {
        Success = true,
        Message = message,
        Data = data
    };
}
