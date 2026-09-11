using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartmentUsers
{
    public class GetDepartmentUsersHandler : IRequestHandler<GetDepartmentUsersCommand, IEnumerable<DepartmentUserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentUsersHandler(IUserRepository userRepository, IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<DepartmentUserDTO>> Handle(GetDepartmentUsersCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByCodeAsync(request.departmentCode);
            var user = await _userRepository.GetByUsernameAsync(request.username);

            var userHasAccess = await _userRepository.UserHasRoleInDepartmentAsync(user!.Id, 0, department!.DepartmentId);

            if (!userHasAccess)
            {
                throw new UnauthorizedAccessException("User does not have access to this department.");
            }

            var users = await _userRepository.GetUsersByDepartmentAsync(department.DepartmentId);

            var departmentUsers = users
                .Select(usr => new DepartmentUserDTO
                {
                    UserId = usr.Id,
                    FullName = $"{usr.FirstName} {usr.LastName}",
                    EmployeeId = usr.EmployeeId,
                    Username = usr.UserName!
                })
                .ToList();

            return departmentUsers;
        }
    }
}
