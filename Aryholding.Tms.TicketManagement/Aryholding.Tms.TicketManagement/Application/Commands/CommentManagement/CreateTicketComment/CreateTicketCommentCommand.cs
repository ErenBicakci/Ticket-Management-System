using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.CommentManagement.CreateTicketComment
{
    public record CreateTicketCommentCommand(CreateTicketCommentRequestDto Dto, string username) : IRequest<bool>
    {
    }
}
