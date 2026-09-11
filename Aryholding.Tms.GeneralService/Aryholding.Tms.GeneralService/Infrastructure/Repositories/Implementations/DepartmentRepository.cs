using Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartmentDashboard;
using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Data;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Implementations
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(GeneralDbContext context) : base(context)
        {
        }

        public async Task<bool> DepartmentIsExitsByCodeAsync(string code)
        {
            return await _context.Departments
                    .Where(dp => dp.DepartmentCode == code)
                    .AnyAsync();
        }

        public async Task<IEnumerable<Department>> GetAllActiveDepartmentsAsync()
        {
            return await _context.Departments
                .Where(d => d.IsActive)
                .ToListAsync();
        }

        public async Task<Department?> GetByCodeAsync(string code)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentCode == code);
        }

        public async Task<DashboardDto> GetDepartmentDashboardAsync(int departmentId)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            var statusCounts = await _context.Tickets
                .Where(t => t.Category.DepartmentId == departmentId)
                .GroupBy(t => t.TicketStatus.TicketStatusCode)
                .Select(g => new { StatusCode = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.StatusCode, g => g.Count);

            var dashboard = new DashboardDto
            {
                DepartmentCode = department?.DepartmentCode,
                DepartmentName = department?.DepartmentName,
                TotalTickets = statusCounts.Values.Sum(),
                PendingTickets = statusCounts.GetValueOrDefault("PENDING"),
                AcceptedTickets = statusCounts.GetValueOrDefault("ACCEPTED"),
                WaitingApprovalTickets = statusCounts.GetValueOrDefault("WAITING_APPROVAL"),
                ApprovedTickets = statusCounts.GetValueOrDefault("APPROVED")
            };

            return dashboard;
        }

    }
}
