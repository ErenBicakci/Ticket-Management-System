namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.UpdateTicket
{
    public class UpdateTicketDto
    {
        public long TicketId { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;

    }
}
