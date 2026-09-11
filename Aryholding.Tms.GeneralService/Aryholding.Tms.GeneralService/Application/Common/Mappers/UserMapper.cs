using Aryholding.Tms.GeneralService.Application.DTOs;
using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Application.Common.Mappers
{
    public class UserMapper
    {
        public static UserDTO MapToResponseDto(ApplicationUser applicationUser)
        {
            return new UserDTO
            {
                Username = applicationUser.UserName!,
                FullName = applicationUser.FullName,
                EmployeeId = applicationUser.EmployeeId,
                Email = applicationUser.Email!,
                DepartmentRoles = UserDepartmentRoleMapper.MapToResponseDtos(applicationUser.UserDepartmentRoles).ToList()
            };
        }

        public static IEnumerable<UserDTO> MapToResponseDtos(IEnumerable<ApplicationUser> applicationUsers)
        {
            return applicationUsers.Select(MapToResponseDto);
        }
    }
}
