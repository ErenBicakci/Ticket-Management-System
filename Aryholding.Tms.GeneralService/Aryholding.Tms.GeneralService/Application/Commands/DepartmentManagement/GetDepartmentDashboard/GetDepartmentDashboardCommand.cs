using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartmentDashboard
{
    public record GetDepartmentDashboardCommand(string departmentCode, string username) : IRequest<DashboardDto>
    {
    }
}
