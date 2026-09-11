using Aryholding.Tms.GeneralService.Application.DTOs;
using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Application.Common.Mappers
{
    public class UserDepartmentRoleMapper
    {
        public static UserDepartmentRoleDTO MapToResponseDto(UserDepartmentRole userDepartmentRole)
        {
            return new UserDepartmentRoleDTO
            {
                DepartmentCode = userDepartmentRole.Department!.DepartmentCode,
                DepartmentName = userDepartmentRole.Department.DepartmentName,
                RoleDescription = userDepartmentRole.Description,
                PriorityLevel = userDepartmentRole.PriorityLevel!.Level,
                PriorityName = userDepartmentRole.PriorityLevel.Name
            };
        }

        public static IEnumerable<UserDepartmentRoleDTO> MapToResponseDtos(IEnumerable<UserDepartmentRole> userDepartmentRoles)
        {
            return userDepartmentRoles.Select(MapToResponseDto);
        }
    }
}
