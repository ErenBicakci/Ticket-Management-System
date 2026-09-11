namespace Aryholding.Tms.TicketManagement.Domain.Entities
{
    public class TicketEventType
    {
        public int Id { get; set; }
        public string EventTypeCode { get; set; } = default!;
        public string DisplayName { get; set; } = default!;

    }
}
