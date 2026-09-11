using Aryholding.Tms.GeneralService.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.UserManagement.GetDepartmentUsersWithDetails
{
    public record GetDepartmentUsersWithDetailsCommand(string username, string departmentCode) : IRequest<IEnumerable<UserDTO>>
    {
    }
}
