using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareManagement.Models;

namespace CareManagement.Models.OM
{
    public class Enum
    {
        public enum EmployeeTitle
        {
            Manager,
            Nurse,
            Librarian,
            Designer
        }

        public enum PaymentType
        {
            Hourly,
            Salary
        }

        public enum EType
        {
            Full_time,
            Part_time,
            On_call
        }

        public enum EStatus
        {
            Resigned,
            Layoff
        }
    }
}
