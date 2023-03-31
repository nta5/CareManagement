using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareManagement.Models.CRM;

namespace CareManagement.Models.SCHDL
{
	public class Invoice
	{
		[Key]
		public Guid InvoiceNumber { get; set; }

		//[Required]
		//[ForeignKey("Renter")]

		//public int RenterId { get; set; }
		//public virtual Renter Renter { get; set; }


        /**
         * once an invoice time period is specified we can change the range of dates allowed for an invoice
         */
        [Required]
        [Range(typeof(DateTime), "1/1/2023", "12/31/2023",
        ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime StartDate { get; set; }

        /**
        * once an invoice time period is specified we can change the range of dates allowed for an invoice
        */
        [Required]
        [Range(typeof(DateTime), "1/1/2023", "12/31/2023",
        ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime EndDate { get; set; }

        /**
		 * The total hours of all the services summed together 
		 */

        [Required]
		[Range(0.01, double.MaxValue)]
		public double TotalHours { get; set; }

		/**
		 * the total cost of all the services summed together
		 */
        [Required]
        [Range(0.01, double.MaxValue)]
        public double TotalCost { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatePaid { get; set; }

    }
}
