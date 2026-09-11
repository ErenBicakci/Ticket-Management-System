namespace Aryholding.Tms.TicketManagement.Application.DTOs
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public IDictionary<string, string[]>? Errors { get; set; }
        public object? Details { get; set; }
    }
}
