using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareManagement.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace CareManagement.Models.OM
{
    public class PayrollTest
    {
        [Key] public Guid PayrollID { get; set; }

        [ForeignKey("Employee")] public Guid EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        
        // [EnumDataType(typeof(EmploymentType))]
        public Enum.EType EmployeeType { get; set; }


        [Range(0, int.MaxValue)] public int Hours { get; set; }

        [Range(0, int.MaxValue)] public int Overtime { get; set; }

        [Range(0, int.MaxValue)] public int? LateDeduction { get; set; }

        [Range(0, int.MaxValue)] public int? VacationPay { get; set; }


        [Range(0, int.MaxValue)] public int? SickPay { get; set; }

        [Range(0, int.MaxValue)] public float? Pre_tax { get; set; }

        [Range(0, int.MaxValue)] public float? Tax { get; set; }

        [Range(0, int.MaxValue)] public int? CheckAmount { get; set; }

        public enum EmploymentType
        {
            F,
            P,
            O
        }
    }
}