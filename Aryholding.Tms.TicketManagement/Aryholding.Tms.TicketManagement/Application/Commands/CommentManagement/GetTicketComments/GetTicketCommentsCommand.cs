using Aryholding.Tms.TicketManagement.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.CommentManagement.GetTicketComments
{
    public record GetTicketCommentsCommand(long ticketId,string username,int page, string? orderDirection) : IRequest<IEnumerable<CommentResponseDto>>
    {
    }
}
