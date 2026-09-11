using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aryholding.Tms.TicketManagement.Domain.Entities
{
    [Table("TicketSeverities")]
    public class TicketSeverity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TicketSeverityCode { get; set; } 
        public string Name { get; set; } = default!;
        
    }
}
