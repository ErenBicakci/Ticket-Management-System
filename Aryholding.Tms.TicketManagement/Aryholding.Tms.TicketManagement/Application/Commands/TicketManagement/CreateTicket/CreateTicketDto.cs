using System.ComponentModel.DataAnnotations;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.CreateTicket
{
    public class CreateTicketDto
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? CategoryCode { get; set; }
        public string? SeverityCode { get; set; }
    }
}
