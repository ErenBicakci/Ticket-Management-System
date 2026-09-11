using Aryholding.Tms.TicketManagement.Application.Common.Exceptions;
using Aryholding.Tms.TicketManagement.Application.Common.Mappers;
using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.GetDepartmentTickets
{
    public class GetDepartmentTicketsHandler : IRequestHandler<GetDepartmentTicketsCommand, IEnumerable<TicketResponseDto>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentTicketsHandler(
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<TicketResponseDto>> Handle(GetDepartmentTicketsCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.GetUserIdByUsernameAsync(request.Username)
                       ?? throw new NotFoundException("User", request.Username);

            var departmentId = await _departmentRepository.GetIdByCodeAsync(request.DepartmentCode)
                             ?? throw new NotFoundException("Department", request.DepartmentCode);

            var userHasRole = await _userRepository.UserHasRoleInDepartmentAsync(userId, 0, departmentId);

            if (!userHasRole)
            {
                throw new UnauthorizedAccessException("User does not have access to this department's tickets.");
            }

            var tickets = await _ticketRepository
                .GetDepartmentTicketsWithFiltersAsync(
                    departmentId,
                    request.SeverityCode,
                    request.StatusCode,
                    request.CategoryCode,
                    request.AssignedUsername,
                    request.OrderDirection,
                    request.Page,
                    10);

            return TicketMapper.MapToResponseDtos(tickets);
        }
    }
}
