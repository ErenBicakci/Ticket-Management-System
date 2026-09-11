namespace Aryholding.Tms.TicketManagement.Application.DTOs
{
    public class UserTicketSummaryDto
    {
        public int TotalTickets { get; set; }
        public int PendingTickets { get; set; }
        public int AcceptedTickets { get; set; }
        public int WaitingApprovalTickets { get; set; }
        public int ApprovedTickets { get; set; }
        public int ActiveTickets => PendingTickets + AcceptedTickets;
    }
}
