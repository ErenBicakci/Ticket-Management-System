namespace Aryholding.Tms.TicketManagement.Application.DTOs
{
    public class TicketResponseDto
    {
        public long TicketId { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string? AssignedUsername { get; set; }
        public int? PriorityLevel { get; set; }
        public string? CategoryCode { get; set; }
        public string? SeverityCode { get; set; }
        public string? TicketStatusCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
