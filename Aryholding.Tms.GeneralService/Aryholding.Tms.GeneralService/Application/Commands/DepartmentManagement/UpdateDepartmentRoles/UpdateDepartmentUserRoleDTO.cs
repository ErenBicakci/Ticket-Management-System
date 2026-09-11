namespace Aryholding.Tms.GeneralService.Application.Commands.DepartmentManagement.UpdateDepartmentRoles
{
    public class UpdateDepartmentUserRoleDTO
    {
        public string UserUsername { get; set; } = default!;
        public int PriorityLevel { get; set; }
    }
}
