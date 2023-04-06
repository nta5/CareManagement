using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareManagement.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace CareManagement.Models.OM
{
    public class Payroll
    {
        [Key] public Guid PayrollID { get; set; }

        [Required] [ForeignKey("Employee")] public Guid EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        // [EnumDataType(typeof(EmploymentType))]
        public Enum.EType EmployeeType { get; set; }


        [Required] [Range(0, int.MaxValue)] public double Hours { get; set; }

        [Required] [Range(0, int.MaxValue)] public double Overtime { get; set; }

        [Range(0, int.MaxValue)] public int? LateDeduction { get; set; }

        [Range(0, int.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:F2}")] 
        public double? VacationPay { get; set; }


        [Range(0, int.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double? SickPay { get; set; }

        [Range(0, int.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:F2}")] 
        public double? Pretax { get; set; }

        [Range(0, int.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:F2}")] 
        public double? Tax { get; set; }

        [Range(0, int.MaxValue)] 
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double? CheckAmount { get; set; }


        public static List<Tuple<DateTime, DateTime>> GetTwoWeekPeriods()
        {
            var startDate = new DateTime(DateTime.Now.Year, 1, 1); // January 1st of current year
            var today = DateTime.Today;
            if (today > startDate)
            {
                startDate = today.AddDays(-(int)today.DayOfWeek); // Get start of current week
            }

            var periods = new List<Tuple<DateTime, DateTime>>();
            var endDate = startDate.AddDays(13);
            while (endDate <= DateTime.Today)
            {
                periods.Add(new Tuple<DateTime, DateTime>(startDate, endDate));
                startDate = endDate.AddDays(1);
                endDate = startDate.AddDays(13);
            }

            return periods;
        }

    }
}