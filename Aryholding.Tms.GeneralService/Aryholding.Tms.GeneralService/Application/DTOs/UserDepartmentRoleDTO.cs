namespace Aryholding.Tms.GeneralService.Application.DTOs
{
    public class UserDepartmentRoleDTO
    {
        public string DepartmentCode { get; set; } = default!;
        public string DepartmentName { get; set; } = default!;
        public string RoleDescription { get; set; } = default!;
        public int PriorityLevel { get; set; }
        public string PriorityName { get; set; } = default!;
    }
}
