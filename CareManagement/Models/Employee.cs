using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareManagement.Models
{
    public class Employee
    {
        [Key]
        [Range(0, int.MaxValue)]

        public int EmployeeId { get; set; }

        [ForeignKey("Qualification")]
        public Guid QualificationId { get; set; }
        public virtual Qualification? Qualification { get; set; }
    
        public int EmpId { get; set; } // Acts as the Primary Key for an employee

        [Required]
        [StringLength(20)]
        public string FName { get; set; } // The employee's first name.

        [Required]
        [StringLength(20)]
        public string LName { get; set; } // The employee's last name.

        [Required]
        [StringLength(50)]
        public string Address { get; set; } // Employee address

        [Required]
        [Phone]
        public string Phone { get; set; } // Employee phone number

        [Required]
        public int EmergencyContact { get; set; } // Employee emergency contact

        [Required]
        public string EmpType { get; set; } // Type of employment e.g. Full/part time, On-Call

        [Required]
        public float PayRate { get; set; } // Hourly rate for pay calculation

        [Required]
        public string PayType { get; set; } // Type of payment for this employee

        public int? VacationDays { get; set; } // Current vacation days available for use

        [Required]
        public string EmpStatus { get; set; } // Employee current status

        public int? SickDays { get; set; } // Current sick days available for use

        [Required]
        public string Title { get; set; } // Employee title. Manager, Nurse

        [Required]
        public DateTime StartDate { get; set; } // When the user was initially hired
    }

}
