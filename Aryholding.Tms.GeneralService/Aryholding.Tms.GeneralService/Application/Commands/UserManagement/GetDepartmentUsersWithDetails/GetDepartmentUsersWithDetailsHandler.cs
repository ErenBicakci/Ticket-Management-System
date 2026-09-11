using Aryholding.Tms.GeneralService.Application.Common.Mappers;
using Aryholding.Tms.GeneralService.Application.DTOs;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.UserManagement.GetDepartmentUsersWithDetails
{
    public class GetDepartmentUsersWithDetailsHandler : IRequestHandler<GetDepartmentUsersWithDetailsCommand, IEnumerable<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentUsersWithDetailsHandler(IUserRepository userRepository, IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<UserDTO>> Handle(GetDepartmentUsersWithDetailsCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByCodeAsync(request.departmentCode);
            if (department == null)
            {
                throw new Exception("Department not found.");
            }

            var user = await _userRepository.GetByUsernameAsync(request.username);
            var userHasAccess = await _userRepository.UserHasRoleInDepartmentAsync(user!.Id, 7, department.DepartmentId);
            if (!userHasAccess)
            {
                throw new UnauthorizedAccessException("User does not have access to this department.");
            }

            var users = await _userRepository.GetUsersByDepartmentAsync(department.DepartmentId);
            return UserMapper.MapToResponseDtos(users);
        }
    }
}
