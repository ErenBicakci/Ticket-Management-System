using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.StatsManagement.GetUserTicketStats
{
    public record GetUserTicketStatsCommand(DateTime? FromUtc, DateTime? ToUtc, string? DepartmentCode) 
        : IRequest<List<UserTicketStatsDto>>;
}
