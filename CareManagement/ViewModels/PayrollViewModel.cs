using CareManagement.Models.SCHDL;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CareManagement.Models.OM;

namespace CareManagement.ViewModels
{
    public class PayrollViewModel
    {
        public Guid SelectedEmployeeId { get; set; }
        public Payroll DisplayedPayroll { get; set; }
        public string PayPeriod { get; set; }
        [Key] public Guid PayrollID { get; set; }


    }
}
