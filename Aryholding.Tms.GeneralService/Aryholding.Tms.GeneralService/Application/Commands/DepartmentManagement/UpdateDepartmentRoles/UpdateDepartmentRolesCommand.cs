using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.UpdateDepartmentRoles
{
    public record UpdateDepartmentRolesCommand(UpdateDepartmentUserRoleDTO Dto, string DepartmentCode, string AdminUsername) : IRequest<bool>
    {
    }
}
