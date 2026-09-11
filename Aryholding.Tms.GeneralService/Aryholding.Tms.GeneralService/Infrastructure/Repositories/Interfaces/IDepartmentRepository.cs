using Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartmentDashboard;
using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        Task<Department?> GetByCodeAsync(string code);

        Task<IEnumerable<Department>> GetAllActiveDepartmentsAsync();

        Task<DashboardDto> GetDepartmentDashboardAsync(int departmentId);

        Task<bool> DepartmentIsExitsByCodeAsync(string code);
    }
}
