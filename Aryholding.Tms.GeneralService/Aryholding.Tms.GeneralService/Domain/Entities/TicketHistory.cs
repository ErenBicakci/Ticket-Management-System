namespace Aryholding.Tms.GeneralService.Domain.Entities
{
    public class TicketHistory
    {
        public int Id { get; set; }

        public long TicketId { get; set; }

        public Ticket Ticket { get; set; } = default!;

        public int TicketEventTypeId { get; set; }

        public TicketEventType TicketEventType { get; set; } = default!;

        public string ChangedByUserId { get; set; } = default!;

        public ApplicationUser ChangedByUser { get; set; } = default!;

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public string? EventMessage { get; set; }

        public int? OldStatusId { get; set; }
        public TicketStatus? OldStatus { get; set; }

        public int? NewStatusId { get; set; }
        public TicketStatus? NewStatus { get; set; }
    }
}
