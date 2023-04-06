using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CareManagement.Models.CRM;

namespace CareManagement.Models.SCHDL
{
	public class Invoice : IValidatableObject
    {
		[Key]
		public Guid InvoiceNumber { get; set; }


        [Required]
        [ForeignKey("Renter")]
        [Display(Name = "Renter")]
        public Guid RenterId { get; set; } // the actual FK in the table
        public virtual Renter? Renter { get; set; } // reference for the FK


        /**
         * once an invoice time period is specified we can change the range of dates allowed for an invoice
         */
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        /**
        * once an invoice time period is specified we can change the range of dates allowed for an invoice
        */
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        /**
		 * The total hours of all the services summed together 
		 */
        [Display(Name = "Total Hours")]
        public double TotalHours { get; set; }

        /**
		 * the total cost of all the services summed together
		 */
        [Display(Name = "Total Cost")]
        public double TotalCost { get; set; }

        [DisplayName("Date Paid")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatePaid { get; set; }

        /**
         * If the invoice is sent to customer or not
         */
        [DisplayName("Sent to Customer")]
        public bool IsSent { get; set; }

        [DisplayName("Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult("End date must be greater than the start date.", new[] { nameof(EndDate) });
            }

            var elapsed = DueDate.Subtract(EndDate);

            if (elapsed.TotalDays > 60)
            {
                yield return new ValidationResult("Due date must be in 60 days from the end date.", new[] { nameof(DueDate) });
            }

            if (DatePaid > DueDate)
            {
                yield return new ValidationResult("Date paid must be before due date.", new[] { nameof(DatePaid) });
            }
        }

    }
}
