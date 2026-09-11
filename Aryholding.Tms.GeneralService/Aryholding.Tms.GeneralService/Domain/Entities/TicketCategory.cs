using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aryholding.Tms.GeneralService.Domain.Entities
{
    [Table("TicketCategories")]
    public class TicketCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string TicketCategoryCode { get; set; } = default!;

        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        public int? PriorityLevelId { get; set; }

        public virtual PriorityLevel? PriorityLevel { get; set; }
    }
}
