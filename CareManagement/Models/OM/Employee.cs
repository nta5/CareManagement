﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareManagement.Models.SCHDL;

namespace CareManagement.Models.OM
{
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; }

<<<<<<< HEAD
        [Required]
=======

>>>>>>> dadd3550b236ad7a3fa6d9c502920e11ae0285e1
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
        public string EmergencyContact { get; set; } // Employee emergency contact

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; } // Employee phone number


        [Required]
<<<<<<< HEAD
        public Enum.EType EmployeeType { get; set; } // Type of employment e.g. Full/part time, On-Call
=======
        public string EmployeeType { get; set; } // Type of employment e.g. Full/part time, On-Call

>>>>>>> dadd3550b236ad7a3fa6d9c502920e11ae0285e1

        [Required]
        [Range(0, float.MaxValue)]
        public float PayRate { get; set; } // Hourly rate for pay calculation

        [Required]
        public Enum.PaymentType PayType { get; set; } // Type of payment for this employee


        [Range(0, int.MaxValue)]
        public int? VacationDays { get; set; } // Current vacation days available for use

        [Required]
<<<<<<< HEAD
        public Enum.EStatus EmployeeStatus { get; set; } // Employee current status
=======
        public string EmployeeStatus { get; set; } // Employee current status
>>>>>>> dadd3550b236ad7a3fa6d9c502920e11ae0285e1


        [Range(0, int.MaxValue)]
        public int? SickDays { get; set; } // Current sick days available for use

        [Required]
<<<<<<< HEAD
        public Enum.EmployeeTitle Title { get; set; } // Employee title. Manager, Nurse
=======
        public DateTime StartDate { get; set; } // When the user was initially hired

        [Required]
        public string Title { get; set; } // Employee title. Manager, Nurse
>>>>>>> dadd3550b236ad7a3fa6d9c502920e11ae0285e1

        [Range(0, int.MaxValue)]
        public int? TotalHoursWorked { get; set; } // Total hours worked since joining company. Used for seniority

    }

}
