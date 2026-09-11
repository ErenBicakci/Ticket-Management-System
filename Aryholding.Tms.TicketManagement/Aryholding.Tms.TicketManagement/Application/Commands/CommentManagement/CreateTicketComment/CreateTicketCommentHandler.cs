using Aryholding.Tms.TicketManagement.Application.Common.Exceptions;
using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.CommentManagement.CreateTicketComment
{
    public class CreateTicketCommentHandler : IRequestHandler<CreateTicketCommentCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ITicketRepository _ticketRepository;
        
        
        public CreateTicketCommentHandler(IUserRepository userRepository, ICommentRepository commentRepository, ITicketRepository ticketRepository)
        {
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<bool> Handle(CreateTicketCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.GetUserIdByUsernameAsync(request.username);
            var ticket = await _ticketRepository.GetByIdAsync(request.Dto.ticketId) ??
                throw new NotFoundException("Ticket not found");
            var userHasRole = await _userRepository.UserHasRoleInDepartmentAsync(userId, 0, ticket.Category.DepartmentId);
            if (!userHasRole)
            {
                throw new UnauthorizedException("You do not have permission to perform this action.");
            }

            var comment = new Comment
            {
                UserId = userId,
                Description = request.Dto.comment,
                TicketId = request.Dto.ticketId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();

            return true;
        }
    }
}
