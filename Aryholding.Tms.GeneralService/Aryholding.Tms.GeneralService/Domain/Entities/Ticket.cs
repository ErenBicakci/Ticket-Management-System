namespace Aryholding.Tms.GeneralService.Domain.Entities
{
    public sealed class Ticket
    {
        public long Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!; 
        public string UserId { get; set; } = default!;     
        public ApplicationUser User { get; set; } = default!;
        public int? PriorityLevelId { get; set; }
        public PriorityLevel? PriorityLevel { get; set; }
        public int? CategoryId { get; set; }

        public string? AssignedUserId { get; set; }
        public ApplicationUser? AssignedUser { get; set; }
        public TicketCategory? Category { get; set; }
        public int? SeverityId { get; set; }
        public TicketSeverity? Severity { get; set; }
        public bool IsActive { get; set; } = true;

        public int? TicketStatusId { get; set; }
        public TicketStatus? TicketStatus { get; set; }

        public ICollection<TicketHistory> TicketHistories { get; set; } = new List<TicketHistory>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
