using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aryholding.Tms.GeneralService.Domain.Entities
{
    [Table("PriorityLevels")]
    public class PriorityLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PriorityLevelId { get; set; }
        public int Level { get; set; }
        public string Name { get; set; } = default!;
        public string PriorityLevelCode { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<UserDepartmentRole> UserDepartmentRoles { get; set; } = new List<UserDepartmentRole>();
    }
}
