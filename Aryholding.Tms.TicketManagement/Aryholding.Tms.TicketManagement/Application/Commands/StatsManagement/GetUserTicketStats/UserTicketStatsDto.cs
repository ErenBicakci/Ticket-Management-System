namespace Aryholding.Tms.TicketManagement.Application.Commands.StatsManagement.GetUserTicketStats
{
    public class UserTicketStatsDto
    {
        public string UserId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public double? AvgMinutesToSubmitForApproval { get; set; }
        public double? AvgMinutesToCompletion { get; set; }
        public int AcceptedTicketCount { get; set; }
        public int SubmittedForApprovalCount { get; set; }
        public int CompletedTicketCount { get; set; }
    }
}
