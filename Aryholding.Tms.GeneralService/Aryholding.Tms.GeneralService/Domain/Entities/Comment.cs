using System;

namespace Aryholding.Tms.GeneralService.Domain.Entities
{
    public class Comment
    {
        public long Id { get; set; }
        public string Description { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
        public long TicketId { get; set; }
        public Ticket Ticket { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
