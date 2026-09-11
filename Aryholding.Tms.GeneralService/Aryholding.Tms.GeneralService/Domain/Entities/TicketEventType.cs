namespace Aryholding.Tms.GeneralService.Domain.Entities
{
    public class TicketEventType
    {
        public int Id { get; set; }
        public string EventTypeCode { get; set; } = default!;
        public string DisplayName { get; set; } = default!;

    }
}
