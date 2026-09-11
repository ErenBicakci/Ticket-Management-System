using System.Text.Json.Serialization;

namespace Aryholding.Tms.TicketManagement.Application.DTOs
{
    public class CommentResponseDto
    {

        public long Id { get; set; }
        public string Description { get; set; } = default!;
        public string Username { get; set; } = default!;
        public long TicketId { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
