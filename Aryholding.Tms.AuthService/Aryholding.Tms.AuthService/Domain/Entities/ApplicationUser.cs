using Microsoft.AspNetCore.Identity;

namespace Aryholding.Tms.AuthService.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string FullName { get { return FirstName + " " + LastName; } }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        
        public string EmployeeId { get; set; } = default!;
        public bool SuperUser { get; set; } = false;
        public ICollection<UserDepartmentRole> UserDepartmentRoles { get; set; } = new List<UserDepartmentRole>();

    }
}
