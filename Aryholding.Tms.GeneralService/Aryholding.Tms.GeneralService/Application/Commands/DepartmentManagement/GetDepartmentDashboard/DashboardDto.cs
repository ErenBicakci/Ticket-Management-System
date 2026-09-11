namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartmentDashboard
{
    public class DashboardDto
    {
        public string? DepartmentCode { get; set; }
        public string? DepartmentName { get; set; }
        public int TotalTickets { get; set; }
        public int PendingTickets { get; set; }
        public int AcceptedTickets { get; set; }
        public int WaitingApprovalTickets { get; set; }
        public int ApprovedTickets { get; set; }
    }
}
