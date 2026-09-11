using Aryholding.Tms.GeneralService.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartments
{
    public record GetDepartmentsCommand(string username) : IRequest<IEnumerable<DepartmentResponseDTO>>
    {
    }
}
