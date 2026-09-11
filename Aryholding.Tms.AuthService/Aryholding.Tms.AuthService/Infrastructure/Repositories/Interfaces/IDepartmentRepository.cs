using Aryholding.Tms.AuthService.Domain.Entities;

namespace Aryholding.Tms.AuthService.Infrastructure.Repositories.Interfaces
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        Task<Department?> GetByNameAsync(string departmentName);
        Task<IEnumerable<Department>> GetActiveDepartmentsAsync();
        Task<IEnumerable<Department>> GetAllDepartmentsIncludingInactiveAsync();
        Task<bool> IsDepartmentNameUniqueAsync(string departmentName, int? excludeDepartmentId = null);
        Task<bool> CanDepartmentBeDeletedAsync(int departmentId);
    }
}