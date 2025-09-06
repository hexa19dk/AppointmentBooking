namespace AgencyBookAppointments.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }
        public DateTime timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<T> Ok(T data, string message, int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Error = null
            };
        }

        public static ApiResponse<T> Fail(string message, int statusCode, T data, string? errorDetail = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Error = errorDetail!,
                Data = data,
                timestamp = DateTime.UtcNow
            };
        }
    }
}
