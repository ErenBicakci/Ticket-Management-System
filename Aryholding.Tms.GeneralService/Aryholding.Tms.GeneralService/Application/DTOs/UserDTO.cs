namespace Aryholding.Tms.GeneralService.Application.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string EmployeeId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public List<UserDepartmentRoleDTO> DepartmentRoles { get; set; } = new List<UserDepartmentRoleDTO>();
    }
}
