using System.ComponentModel.DataAnnotations;

namespace CareManagement.Models.OM
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        [Required]
        public bool IsPresent { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
