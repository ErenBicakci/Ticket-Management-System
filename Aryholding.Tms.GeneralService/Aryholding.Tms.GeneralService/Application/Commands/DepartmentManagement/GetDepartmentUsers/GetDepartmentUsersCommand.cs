using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartmentUsers
{
    public record GetDepartmentUsersCommand(string departmentCode, string username) : IRequest<IEnumerable<DepartmentUserDTO>>
    {
    }
}
