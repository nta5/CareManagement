using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareManagement.Data;
using CareManagement.Models.SCHDL;
using CareManagement.Models.CRM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CareManagement.Controllers.SCHDL
{
    public class InvoicesController : Controller
    {
        private readonly CareManagementContext _context;

        public InvoicesController(CareManagementContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index(string sortOrder)
        {
            var careManagementContext = _context.Invoice.Include(i => i.Renter);

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["StartDateSortParm"] = sortOrder == "StartDate" ? "date_desc" : "StartDate";
            ViewData["EndDateSortParm"] = sortOrder == "EndDate" ? "end_date_desc" : "EndDate";
            ViewData["TotalHoursSortParm"] = sortOrder == "TotalHours" ? "total_hrs_desc" : "TotalHours";
            ViewData["TotalCostSortParm"] = sortOrder == "TotalCost" ? "total_cost_desc" : "TotalCost";
            ViewData["DatePaidSortParm"] = sortOrder == "DatePaid" ? "date_paid_desc" : "DatePaid";
            ViewData["IsSentSortParm"] = sortOrder == "IsSent" ? "is_sent_false" : "IsSent";
            ViewData["DueDateSortParm"] = sortOrder == "DueDate" ? "due_date_desc" : "DueDate";
            var invoices = from i in careManagementContext
                           select i;
            switch (sortOrder)
            {
                case "name_desc":
                    invoices = invoices.OrderByDescending(i => i.Renter.Name);
                    break;
                case "StartDate":
                    invoices = invoices.OrderBy(i => i.StartDate);
                    break;
                case "date_desc":
                    invoices = invoices.OrderByDescending(i => i.StartDate);
                    break;
                case "EndDate":
                    invoices = invoices.OrderBy(i => i.EndDate);
                    break;
                case "end_date_desc":
                    invoices = invoices.OrderByDescending(i => i.EndDate);
                    break;
                case "TotalHours":
                    invoices = invoices.OrderBy(i => i.TotalHours);
                    break;
                case "total_hrs_desc":
                    invoices = invoices.OrderByDescending(i => i.TotalHours);
                    break;
                case "TotalCost":
                    invoices = invoices.OrderBy(i => i.TotalCost);
                    break;
                case "total_cost_desc":
                    invoices = invoices.OrderByDescending(i => i.TotalCost);
                    break;
                case "DatePaid":
                    invoices = invoices.OrderBy(i => i.DatePaid);
                    break;
                case "date_paid_desc":
                    invoices = invoices.OrderByDescending(i => i.DatePaid);
                    break;
                case "DueDate":
                    invoices = invoices.OrderBy(i => i.DueDate);
                    break;
                case "due_date_desc":
                    invoices = invoices.OrderByDescending(i => i.DueDate);
                    break;
                case "IsSent":
                    invoices = invoices.Where(i => i.IsSent == true);
                    break;
                case "is_sent_false":
                    invoices = invoices.Where(i => i.IsSent == false);
                    break;
                default:
                    invoices = invoices.OrderBy(i => i.StartDate);
                    break;
            }

            return View(await invoices.AsNoTracking().ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Invoice.Include(x => x.Renter) == null)
            {
                return NotFound();
            }

            var invoice = _context.Invoice
                .Include(x => x.Renter)
                .Select(i => new InvoiceViewModel
                {
                    Renter = i.Renter,
                    InvoiceNumber = i.InvoiceNumber,
                    StartDate = i.StartDate,
                    EndDate = i.EndDate,
                    TotalCost = i.TotalCost,
                    TotalHours = i.TotalHours,
                    DatePaid = i.DatePaid,
                    IsSent = i.IsSent,
                    DueDate = i.DueDate,
                    RenterId = i.RenterId,
                    ServiceHours = _context.Schedule
                    .Where(s => s.RenterId == i.RenterId)
                    .Select(s => s.Service.Hours).FirstOrDefault(),
                    ServiceType = _context.Schedule
                    .Where(s => s.RenterId == i.RenterId)
                    .Select(s => s.Service.Type).FirstOrDefault(),
                    ServiceRate = _context.Schedule
                    .Where(s => s.RenterId == i.RenterId)
                    .Select(s => s.Service.Rate).FirstOrDefault()
                });

            var invoice_detail = await invoice.FirstOrDefaultAsync(m => m.InvoiceNumber == id);


            if (invoice_detail == null)
            {
                return NotFound();
            }

            return View(invoice_detail);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["RenterId"] = _context.Renter.Select(r => new SelectListItem
            {
                Value = r.RenterId.ToString(),
                Text = $"{r.Name} ({r.RmNumber})"
            }).ToList();
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceNumber,RenterId,StartDate,EndDate,TotalHours,TotalCost,DatePaid,IsSent,DueDate")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                invoice.InvoiceNumber = Guid.NewGuid();
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RenterId"] = _context.Renter.Select(r => new SelectListItem
            {
                Value = r.RenterId.ToString(),
                Text = $"{r.Name} ({r.RmNumber})"
            }).ToList();
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Invoice == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["RenterId"] = _context.Renter.Select(r => new SelectListItem
            {
                Value = r.RenterId.ToString(),
                Text = $"{r.Name} ({r.RmNumber})"
            }).ToList();
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("InvoiceNumber,RenterId,StartDate,EndDate,TotalHours,TotalCost,DatePaid,IsSent,DueDate")] Invoice invoice)
        {
            if (id != invoice.InvoiceNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RenterId"] = _context.Renter.Select(r => new SelectListItem
            {
                Value = r.RenterId.ToString(),
                Text = $"{r.Name} ({r.RmNumber})"
            }).ToList();
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Invoice == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.Renter)
                .FirstOrDefaultAsync(m => m.InvoiceNumber == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Invoice == null)
            {
                return Problem("Entity set 'CareManagementContext.Invoice'  is null.");
            }
            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoice.Remove(invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(Guid id)
        {
            return (_context.Invoice?.Any(e => e.InvoiceNumber == id)).GetValueOrDefault();
        }

        // GET: Invoices/Email/5
        public async Task<IActionResult> Email(Guid? id)
        {
            if (id == null || _context.Invoice.Include(x => x.Renter) == null)
            {
                return NotFound();
            }

            var invoice = _context.Invoice
                .Include(x => x.Renter)
                .Select(i => new InvoiceViewModel
                {
                    Renter = i.Renter,
                    InvoiceNumber = i.InvoiceNumber,
                    StartDate = i.StartDate,
                    EndDate = i.EndDate,
                    TotalCost = i.TotalCost,
                    TotalHours = i.TotalHours,
                    DatePaid = i.DatePaid,
                    IsSent = i.IsSent,
                    DueDate = i.DueDate,
                    RenterId = i.RenterId,
                    ServiceHours = _context.Schedule
                    .Where(s => s.RenterId == i.RenterId)
                    .Select(s => s.Service.Hours).FirstOrDefault(),
                    ServiceType = _context.Schedule
                    .Where(s => s.RenterId == i.RenterId)
                    .Select(s => s.Service.Type).FirstOrDefault(),
                    ServiceRate = _context.Schedule
                    .Where(s => s.RenterId == i.RenterId)
                    .Select(s => s.Service.Rate).FirstOrDefault()
                });

            var invoice_email = await invoice.FirstOrDefaultAsync(m => m.InvoiceNumber == id);


            if (invoice_email == null)
            {
                return NotFound();
            }

            // Invoice is sent to Renter
            invoice_email.IsSent = true;

            var content = "Invoice From Care Management: \n"
                + "| Renter: " + invoice_email.Renter.Name + "\n"
                + "| Invoice Number: " + invoice_email.InvoiceNumber + "\n"
                + "| Start Date: " + invoice_email.StartDate + "\n"
                + "| End Date: " + invoice_email.EndDate + "\n"
                + "| Total Hours: " + invoice_email.TotalHours + "\n"
                + "| Total Cost: " + invoice_email.TotalCost + "\n"
                + "| Date Paid: " + invoice_email.DatePaid + "\n"
                + "| Sent To Renter: " + (invoice_email.IsSent ? "Yes" : "No") + "\n"
                + "| Due Date: " + invoice_email.DueDate + "\n"
                + "| Service Type: " + invoice_email.ServiceType + "\n"
                + "| Service Rate: " + invoice_email.ServiceRate + "\n"
                + "| Service Hours: " + invoice_email.ServiceHours + "\n";

            TempData["Invoice"] = content;
            TempData["CustomerEmail"] = invoice_email.Renter.Email;

            return RedirectToAction("Index", "Email");
        }
    }

    internal class InvoiceViewModel
    {
        [DisplayName("Invoice Number")]
        public Guid InvoiceNumber { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Total Hours")]
        public double TotalHours { get; set; }
        [Display(Name = "Total Cost")]
        public double TotalCost { get; set; }
        [DisplayName("Date Paid")]
        public DateTime DatePaid { get; set; }
        [DisplayName("Sent to Customer")]
        public bool IsSent { get; set; }
        [DisplayName("Due Date")]
        public DateTime DueDate { get; set; }
        [DisplayName("Service Type")]
        public string ServiceType { get; set; }
        [DisplayName("Service Rate")]
        public double ServiceRate { get; set; }
        [DisplayName("Service Hours")]
        public double ServiceHours { get; set; }

        [Display(Name = "Renter")]
        public Guid RenterId { get; set; }
        public Renter? Renter { get; internal set; }

    }
}
