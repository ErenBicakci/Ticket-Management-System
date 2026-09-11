using System.Text.Json.Serialization;

namespace Aryholding.Tms.AuthService.Application.DTOs
{
    public class ErrorResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; } = false;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }

        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("details")]
        public object? Details { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, string[]>? Errors { get; set; }

        public ErrorResponse()
        {
        }

        public ErrorResponse(string message, int statusCode, string? errorCode = null)
        {
            Message = message;
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}
