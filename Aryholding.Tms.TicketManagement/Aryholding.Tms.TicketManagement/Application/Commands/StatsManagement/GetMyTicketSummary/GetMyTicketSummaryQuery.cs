using Aryholding.Tms.TicketManagement.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.StatsManagement.GetMyTicketSummary
{
    public record GetMyTicketSummaryQuery(string Username) : IRequest<UserTicketSummaryDto>;
}
