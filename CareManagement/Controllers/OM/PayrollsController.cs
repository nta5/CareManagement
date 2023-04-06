using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareManagement.Data;
using CareManagement.Models.OM;

namespace CareManagement.Controllers.OM
{
    public class PayrollsController : Controller
    {
        private readonly CareManagementContext _context;

        public PayrollsController(CareManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get the start and end dates for the past two weeks
            DateTime startDate = DateTime.Parse("2023-03-20");
            DateTime endDate = DateTime.Parse("2023-04-20");

            // Get all shifts within the past two weeks
            var shifts = _context.Shift
                .Where(s => s.StartTime >= startDate && s.EndTime <= endDate && s.Employee.FirstName == "Bruce")
                .Include(s => s.Employee)
                .ToList();

            // Calculate the total hours worked and pay for each employee            

            var employeeShifts = shifts.GroupBy(s => s.EmployeeId)
                .Select(g => new
                {
                    EmployeeId = g.Key,                    
                    TotalHoursWorked = g.Sum(s => (s.EndTime - s.StartTime).TotalHours),
                    PayRate = _context.Employee.Where(e => e.EmployeeId == g.Key).Select(e => e.PayRate).FirstOrDefault(),
                    SickDays = g.Sum(s => s.Sick ? 1 : 0)
                })
                .FirstOrDefault();

            Guid empId = employeeShifts.EmployeeId;
            double totalHours = employeeShifts.TotalHoursWorked;
            double payRate = employeeShifts.PayRate;
            int sickDays = employeeShifts.SickDays;

            var currentEmployee = _context.Employee.Where(e => e.EmployeeId == empId).FirstOrDefault();
            var sickPay = 0;
            var sickHours = sickDays * 8;
            if (currentEmployee.SickDays >= sickDays)
            {
                sickPay += sickHours;
                currentEmployee.SickDays -= sickDays;
                totalHours -= sickHours;
                _context.SaveChanges();
            }
            else
            {
                totalHours -= sickHours;
            }



            // Calculate the total payroll for all employees
            int totalPayroll = (int) (totalHours * payRate);

            // Return the payroll data as a view model
            
            //var payrollidTemp = new Guid();
            var checker = _context.Payroll.Where(s => s.StartDate >= startDate && s.EndDate <= endDate && s.Hours == totalHours && s.SickPay == sickPay ).ToList();
            if (!checker.Any())
            {
                var payrollsToDelete = _context.Payroll
                    .Where(s => s.StartDate >= startDate && s.EndDate <= endDate && s.StartDate != s.EndDate)
                    .ToList();

                if (payrollsToDelete.Any())
                {
                    _context.Payroll.RemoveRange(payrollsToDelete);
                    _context.SaveChanges();
                }
                _context.Payroll.AddRange(
                new Payroll
                {
                    PayrollID = new Guid(),
                    EmployeeId = empId,
                    StartDate = startDate,
                    EndDate = endDate,
                    EmployeeType = Models.OM.Enum.EType.Part_time,
                    Hours = (int)totalHours,
                    Overtime = 10,
                    LateDeduction = 1,
                    VacationPay = 1,
                    SickPay = sickPay,
                    Pre_tax = 100,
                    Tax = 10,
                    //EmployeePayroll = employeePayroll,
                    CheckAmount = totalPayroll
                }

                );
                _context.SaveChanges();
            }
            
            
            var payrollView = await _context.Payroll.ToListAsync();
            

            return View(payrollView);
        }






        // GET: Payrolls
        /*public async Task<IActionResult> Index()
        {
            var bruceShifts = _context.Shift.Where(s => s.Employee.FirstName == "Bruce").Include(s => s.Employee);
            var careManagementContext = _context.Payroll.Include(p => p.Employee);
            return View(await careManagementContext.ToListAsync());
        }*/

        // GET: Payrolls/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Payroll == null)
            {
                return NotFound();
            }

            var payroll = await _context.Payroll
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.PayrollID == id);
            if (payroll == null)
            {
                return NotFound();
            }

            return View(payroll);
        }

        // GET: Payrolls/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "Address");
            return View();
        }

        // POST: Payrolls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PayrollID,EmployeeId,StartDate,EndDate,EmployeeType,Hours,Overtime,LateDeduction,VacationPay,SickPay,Pre_tax,Tax,CheckAmount")] Payroll payroll)
        {
            if (ModelState.IsValid)
            {
                payroll.PayrollID = Guid.NewGuid();
                _context.Add(payroll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "Address", payroll.EmployeeId);
            return View(payroll);
        }

        // GET: Payrolls/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Payroll == null)
            {
                return NotFound();
            }

            var payroll = await _context.Payroll.FindAsync(id);
            if (payroll == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "Address", payroll.EmployeeId);
            return View(payroll);
        }

        // POST: Payrolls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PayrollID,EmployeeId,StartDate,EndDate,EmployeeType,Hours,Overtime,LateDeduction,VacationPay,SickPay,Pre_tax,Tax,CheckAmount")] Payroll payroll)
        {
            if (id != payroll.PayrollID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payroll);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayrollExists(payroll.PayrollID))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "Address", payroll.EmployeeId);
            return View(payroll);
        }

        // GET: Payrolls/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Payroll == null)
            {
                return NotFound();
            }

            var payroll = await _context.Payroll
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.PayrollID == id);
            if (payroll == null)
            {
                return NotFound();
            }

            return View(payroll);
        }

        // POST: Payrolls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Payroll == null)
            {
                return Problem("Entity set 'CareManagementContext.Payroll'  is null.");
            }
            var payroll = await _context.Payroll.FindAsync(id);
            if (payroll != null)
            {
                _context.Payroll.Remove(payroll);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayrollExists(Guid id)
        {
          return (_context.Payroll?.Any(e => e.PayrollID == id)).GetValueOrDefault();
        }


    }
}
