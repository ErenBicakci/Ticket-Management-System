namespace Aryholding.Tms.GeneralService.Domain.Entities
{
    public sealed class TicketStatus
    {
        public int Id { get; set; }

        public string TicketStatusCode { get; set; } = default!;

        public string Name { get; set; } = default!;


    }
}
