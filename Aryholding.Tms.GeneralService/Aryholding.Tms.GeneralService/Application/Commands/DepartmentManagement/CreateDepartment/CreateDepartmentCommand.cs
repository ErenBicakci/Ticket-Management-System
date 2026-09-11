using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.CreateDepartment
{
    public record CreateDepartmentCommand(CreateDepartmentDTO dto, string username) : IRequest<bool>
    {
    }
}
