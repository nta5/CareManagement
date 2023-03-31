using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareManagement.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace CareManagement.Models.OM
{
    public class Payroll
    {
        [Key]
        public int PAY_ID { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Required]
        public DateTime START_DATE { get; set; }

        public DateTime? END_DATE { get; set; }

        [Required]
        [EnumDataType(typeof(EmploymentType))]
        public string EMP_TYPE { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int HOURS { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int OVERTIME { get; set; }

        [Range(0, int.MaxValue)]
        public int HOUR_RATE { get; set; }

        [Range(0, int.MaxValue)]
        public int CHECK_AMOUNT { get; set; }


    }

    public enum EmploymentType
    {
        F,
        P,
        O
    }
}