using Aryholding.Tms.TicketManagement.Application.Common.Mappers;
using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Application.Common.Exceptions;
using Aryholding.Tms.TicketManagement.Application.Common.Mappers;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketHistoryManagement.GetTicketHistory
{
    public class GetTicketHistoryHandler : IRequestHandler<GetTicketHistoryCommand, IEnumerable<TicketHistoryResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketHistoryRepository _ticketHistoryRepository;

        public GetTicketHistoryHandler(IUserRepository userRepository, ITicketRepository ticketRepository, ITicketHistoryRepository ticketHistoryRepository)
        {
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _ticketHistoryRepository = ticketHistoryRepository;
        }

        public async Task<IEnumerable<TicketHistoryResponseDto>> Handle(GetTicketHistoryCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.GetUserIdByUsernameAsync(request.username);
            var ticket = await _ticketRepository.GetByIdAsync(request.ticketId) ??
                throw new NotFoundException("Ticket not found");

            var userHasRole = await _userRepository.UserHasRoleInDepartmentAsync(userId, 0, ticket.Category.DepartmentId);

            if (!userHasRole)
            {
                throw new UnauthorizedAccessException("UnauthorizedAccesException");
            }

            var ticketHistories = await _ticketHistoryRepository.GetAllByTicketIdAsync(request.ticketId);
            return TicketHistoryMapper.MapToResponseDtos(ticketHistories);
        }
    }
}
