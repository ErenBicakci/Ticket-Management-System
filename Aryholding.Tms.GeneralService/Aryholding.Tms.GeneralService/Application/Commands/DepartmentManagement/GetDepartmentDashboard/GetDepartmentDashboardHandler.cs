using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartmentDashboard
{
    public class GetDepartmentDashboardHandler : IRequestHandler<GetDepartmentDashboardCommand, DashboardDto>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IUserRepository _userRepository;

        public GetDepartmentDashboardHandler(IDepartmentRepository departmentRepository, ITicketCategoryRepository ticketCategoryRepository, IUserRepository userRepository)
        {
            _departmentRepository = departmentRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _userRepository = userRepository;
        }

        public async Task<DashboardDto> Handle(GetDepartmentDashboardCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByCodeAsync(request.departmentCode);
            var user = await _userRepository.GetByUsernameAsync(request.username);
            var userHasRole = await _userRepository.UserHasRoleInDepartmentAsync(user!.Id, 0, department!.DepartmentId);

            if (department == null || !userHasRole)
            {
                throw new Exception("Department not found or user does not have access to this department.");
            }

            var departmentDashboard = await _departmentRepository.GetDepartmentDashboardAsync(department.DepartmentId);
            return departmentDashboard;
        }
    }
}
