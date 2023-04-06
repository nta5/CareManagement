using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareManagement.Models.SCHDL;

namespace CareManagement.Models.OM
{
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; }

        [Required]
        [ForeignKey("Qualification")]
        public Guid QualificationId { get; set; }
        public virtual Qualification? Qualification { get; set; }

        [Required]
        [StringLength(20)]
        public string? FirstName { get; set; } // The employee's first name.

        [Required]
        [StringLength(20)]
        public string? LastName { get; set; } // The employee's last name.

        [Required]
        [StringLength(50)]
        public string? Address { get; set; } // Employee address


        [Required]
        public int EmergencyContact { get; set; } // Employee emergency contact

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; } // Employee phone number


        [Required]
        public Enum.EType EmployeeType { get; set; } // Type of employment e.g. Full/part time, On-Call


        [Required]
        [Range(0, float.MaxValue)]
        public float PayRate { get; set; } // Hourly rate for pay calculation

        [Required]
        public Enum.PaymentType PayType { get; set; } // Type of payment for this employee


        [Range(0, int.MaxValue)]
        public int? VacationDays { get; set; } // Current vacation days available for use

        [Required]
        public Enum.EStatus EmployeeStatus { get; set; } // Employee current status



        [Range(0, int.MaxValue)]
        public int SickDays { get; set; } // Current sick days available for use

        [Required]
        public Enum.EmployeeTitle Title { get; set; } // Employee title. Manager, Nurse

        [Required]
        public DateTime StartDate { get; set; } // When the user was initially hired


        [Range(0, int.MaxValue)]
        public int? TotalHoursWorked { get; set; } // Total hours worked since joining company. Used for seniority
        public string FirstNameLastName => FirstName + " " + LastName;
    }

}