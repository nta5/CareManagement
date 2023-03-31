
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareManagement.Models.OM;

namespace CareManagement.Models.SCHDL
{
    public class Qualification
    {
        [Key]
        public Guid QualificationId { get; set; }


        //[Required]
        //[ForeignKey("Employee")]
        //public Guid EmployeeId { get; set; }
        //public virtual Employee Employee { get; set; }

        [Required]
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }

        [Required]
        [StringLength(50)]
        public string QualificationDescription { get; set; }
    }
}
