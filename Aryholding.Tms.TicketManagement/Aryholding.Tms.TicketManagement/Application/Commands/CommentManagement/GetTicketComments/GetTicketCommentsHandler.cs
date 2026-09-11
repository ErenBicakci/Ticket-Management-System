
using Aryholding.Tms.TicketManagement.Application.Common.Exceptions;
using Aryholding.Tms.TicketManagement.Application.Common.Mappers;
using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.CommentManagement.GetTicketComments
{
    public class GetTicketCommentsHandler : IRequestHandler<GetTicketCommentsCommand, IEnumerable<CommentResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ITicketRepository _ticketRepository;
        
        public GetTicketCommentsHandler(IUserRepository userRepository, ICommentRepository commentRepository, ITicketRepository ticketRepository)
        {
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<IEnumerable<CommentResponseDto>> Handle(GetTicketCommentsCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetByUsernameAsync(request.username);
            var ticket = await _ticketRepository.GetTicketByIdAsync(request.ticketId) ??
                throw new NotFoundException("Ticket Id : " + request.ticketId + " not found");
            var userHasRole = await _userRepository.UserHasRoleInDepartmentAsync(user.Id, 0, ticket.Category.DepartmentId);
            if (!userHasRole)
            {
                throw new UnauthorizedAccessException("UnauthorizedAccesException");
            }

            var comments = await _commentRepository.GetCommentsByTicketIdWithPagination(ticket.Id, request.orderDirection, request.page, 10);


            return CommentMapper.MapToResponseDtos(comments);
        }
    }
}
