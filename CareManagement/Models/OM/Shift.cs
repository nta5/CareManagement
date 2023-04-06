using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareManagement.Models.SCHDL;

namespace CareManagement.Models.OM
{
    public class Shift
    {
        public Shift()
        {
            this.Schedules = new HashSet<Schedule>();
        }

        [Key]
        public Guid ShiftId { get; set; } // The Shift ID

        [Required]
        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid ManagerId { get; set; }
        public virtual Employee? Employee { get; set; }

        //[Required]
        //public int ManagerId { get; set; } // Whoever is responsible for the employee

        //[ForeignKey("ManagerId")]
        //public Employee Manager { get; set; } // Navigation property for the manager

        [Required]
        public DateTime StartTime { get; set; } // When the shift starts

        [Required]
        public DateTime EndTime { get; set; } // When the shift ends

        

        [Required]
        public bool Sick { get; set; } // Is true if employee called in sick

        [Required]
        [ForeignKey("Schedule")]
        public Guid ScheduleId { get; set; } // the actual FK in the table
        public virtual ICollection<Schedule>? Schedules { get; set; } // reference for the FK
    }

}