using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Aryholding.Tms.AuthService.Domain.Entities
{
    [Table("UserDepartmentRoles")]
    public class UserDepartmentRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleId { get; set; }

        public string UserId { get; set; } = default!;

        [JsonIgnore] 
        public virtual ApplicationUser user { get; set; } = default!; 

        public int DepartmentId { get; set; }
        
        public virtual Department? Department { get; set; }


        public string Description { get; set; } = default!;

        public int PriorityLevelId { get; set; }
        
        public virtual PriorityLevel? PriorityLevel { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsActive { get; set; }

    }
}
