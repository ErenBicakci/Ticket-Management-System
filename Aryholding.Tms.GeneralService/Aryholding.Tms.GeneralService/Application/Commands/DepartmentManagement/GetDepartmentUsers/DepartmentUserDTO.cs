using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.GetDepartmentUsers
{
    public class DepartmentUserDTO
    {
        public string UserId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string EmployeeId { get; set; } = default!;
        public UserDepartmentRole? DepartmentRole { get; set; }
    }
}
