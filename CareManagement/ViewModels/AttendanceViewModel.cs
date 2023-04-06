using Microsoft.AspNetCore.Mvc.Rendering;
using CareManagement.Models.OM;

namespace CareManagement.ViewModels
{
    public class AttendanceViewModel
    {
        public Guid EmployeeId { get; set; }

        public Attendance currentAttendance { get; set; }
        public SelectList EmployeeList { get; set; }

        public bool IsCheckedIn { get; set; }
    }
}
