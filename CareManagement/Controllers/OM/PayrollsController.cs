using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareManagement.Data;
using CareManagement.Models.OM;
using CareManagement.Utilities;
using CareManagement.Models.SCHDL;
using CareManagement.ViewModels;
using Org.BouncyCastle.Ocsp;

namespace CareManagement.Controllers.OM
{
    public class PayrollsController : Controller
    {
        private readonly CareManagementContext _context;
        private static double tax15Hourly = 25.65;
        private static double tax20Hourly = 53.21;
        private static double tax15Salary = 53359;
        private static double tax20Salary = 106717;
        private static double tax15 = 0.15;
        private static double tax20 = 0.2;
        private static double tax26 = 0.26;

        public PayrollsController(CareManagementContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName");
            List<String> periods = TwoWeekHelper.GetTwoWeekPeriods();
            ViewBag.TwoWeekPeriods = new SelectList(periods);
           /* var currentModel = new PayrollViewModel
            {
                PayrollID = Guid.NewGuid()
            };
            */
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(PayrollViewModel model)
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName");
            List<String> periods = TwoWeekHelper.GetTwoWeekPeriods();
            ViewBag.TwoWeekPeriods = new SelectList(periods);
            // Get the start and end dates for the past two weeks

            string[] dates = model.PayPeriod.Split(" - ");
            DateTime startDate = DateTime.Parse(dates[0].Trim());
            DateTime endDate = DateTime.Parse(dates[1].Trim());

            Guid currentEmployeeId = model.SelectedEmployeeId;
            // Get all shifts within the past two weeks
            var shifts = _context.Shift
                .Where(s => s.StartTime >= startDate && s.EndTime <= endDate && s.Employee.EmployeeId == currentEmployeeId)
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
                        
            
            var currentEmployee = _context.Employee.Where(e => e.EmployeeId == currentEmployeeId).FirstOrDefault();
            if (employeeShifts != null)
            {
                double totalHours = employeeShifts.TotalHoursWorked;
                double payRate = employeeShifts.PayRate;

                // Calculating Sickpay               
                int sickDays = employeeShifts.SickDays;
                int sickHours = 0;
                int unpaidSickDays = 0;
                if (sickDays > currentEmployee.SickDays)
                {
                    unpaidSickDays = sickDays - currentEmployee.SickDays;
                    sickHours = currentEmployee.SickDays * 8;
                    totalHours -= (unpaidSickDays * 8) + sickHours;
                } else
                {
                    sickHours = sickDays * 8;
                    totalHours -= sickHours;
                }

                /*   // Calculating vacation pay
                   var totalVacationDays = _context.Vacation
                    .Where(s => s.StartDate >= startDate && s.EndDate <= endDate && s.Employee.EmployeeId == currentEmployeeId)
                    .Sum(s => (s.EndDate - s.StartDate).TotalDays);
                   double vacationPay = totalVacationDays * 8 * payRate;*/
                var vacationRecords = _context.Vacation
                  .Where(v => v.Employee.EmployeeId == currentEmployeeId &&
                      v.StartDate <= endDate && v.EndDate >= startDate)
                  .ToList();

                var totalVacationDays = vacationRecords
                    .Sum(v =>
                    {
                        var vacationStart = v.StartDate < startDate ? startDate : v.StartDate;
                        var vacationEnd = v.EndDate > endDate ? endDate : v.EndDate;
                        return (vacationEnd - vacationStart).TotalDays;
                    });
                double vacationPay = totalVacationDays * 8 * payRate;

                // Calculate overtime
                double totalOvertimeHours = 0;
                Dictionary<DateTime, double> dailyHours = new Dictionary<DateTime, double>();
                foreach (var shift in shifts)
                {
                    DateTime date = shift.StartTime.Date;
                    if (!dailyHours.ContainsKey(date))
                    {
                        dailyHours[date] = (shift.EndTime - shift.StartTime).TotalHours;
                    }
                    else
                    {
                        dailyHours[date] += (shift.EndTime - shift.StartTime).TotalHours;
                    }
                }
                foreach (var hours in dailyHours.Values)
                {
                    if (hours > 8)
                    {
                        totalOvertimeHours += hours - 8;
                    }
                }

                if (totalHours > 80)
                {
                    double excessHours = totalHours - 80;

                    if (totalOvertimeHours < excessHours)
                    {
                        totalOvertimeHours = excessHours;
                    }
                    totalHours -= totalOvertimeHours;

                }
                else
                {
                    totalHours -= totalOvertimeHours;
                }


                // Calculating totalPay
                double regularPay = 0;
                double overtimePay = 0;
                double sickPay = 0;
                double totalPay = 0;
                double taxBracket = 0;
                if (currentEmployee.PayType == CareManagement.Models.OM.Enum.PaymentType.Hourly)
                {
                    // Calculate total pay
                    regularPay = totalHours * payRate;
                    overtimePay = totalOvertimeHours * (payRate * 1.5);
                    sickPay = sickHours * payRate;
                    totalPay = regularPay + overtimePay + sickPay + vacationPay;
                    if (payRate < tax15Hourly)
                    {
                        taxBracket = tax15;
                    }
                    else if (payRate < tax20Hourly)
                    {
                        taxBracket = tax20;
                    }
                    else
                    {
                        taxBracket = tax26;
                    }
                }
                else
                {
                    totalPay = payRate / 12;
                    if (totalOvertimeHours > 0)
                    {
                        totalPay += (payRate / 2080) * totalOvertimeHours;
                    }
                    if (payRate < tax15Salary)
                    {
                        taxBracket = tax15;
                    }
                    else if (payRate < tax20Salary)
                    {
                        taxBracket = tax20;
                    }
                    else
                    {
                        taxBracket = tax26;
                    }
                }

                Guid payrollId = Guid.NewGuid();
                model.PayrollID = payrollId;

                var checker = _context.Payroll.Where(s => s.PayrollID == payrollId).ToList();

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
                    model.DisplayedPayroll = new Payroll();
                    model.PayrollID = payrollId;
                    model.DisplayedPayroll.PayrollID = payrollId;
                    model.DisplayedPayroll.Employee = currentEmployee;
                    model.DisplayedPayroll.EmployeeId = currentEmployeeId;
                    model.DisplayedPayroll.StartDate = startDate;
                    model.DisplayedPayroll.EndDate = endDate;
                    model.DisplayedPayroll.EmployeeType = currentEmployee.EmployeeType;
                    model.DisplayedPayroll.Hours = totalHours;
                    model.DisplayedPayroll.Overtime = totalOvertimeHours;
                    model.DisplayedPayroll.VacationPay = vacationPay;
                    model.DisplayedPayroll.SickPay = Math.Round(sickPay);
                    model.DisplayedPayroll.Pretax = Math.Round(totalPay, 2);
                    model.DisplayedPayroll.Tax = Math.Round(totalPay * taxBracket, 2);
                    model.DisplayedPayroll.CheckAmount = Math.Round(totalPay - (totalPay * taxBracket), 2);
                    _context.Payroll.AddRange(
                        new Payroll
                        {

                            EmployeeId = currentEmployeeId,
                            PayrollID = payrollId,

                            StartDate = startDate,
                            EndDate = endDate,
                            EmployeeType = currentEmployee.EmployeeType,
                            Hours = totalHours,
                            Overtime = totalOvertimeHours,
                            LateDeduction = 0,
                            VacationPay = Math.Round(vacationPay, 2),
                            SickPay = Math.Round(sickPay, 2),
                            Pretax = Math.Round(totalPay, 2),
                            Tax = Math.Round(totalPay * taxBracket, 2),
                            CheckAmount = Math.Round(totalPay - (totalPay * taxBracket), 2)
                        });
                        _context.SaveChanges();
                    return View(model);
                }                
            }
            return View();
        }




        // GET: Payrolls/Details/5
        [HttpGet]
    public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Payroll == null)
            {
                return NotFound();
            }

            var payroll = await _context.Payroll           
                .Where(p => p.PayrollID == id)
                .FirstOrDefaultAsync();
            if (payroll == null)
            {
                return NotFound();
            }

            return View(payroll);
        }

        // GET: Payrolls/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName");
            return View();
        }

        // POST: Payrolls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewPayroll([Bind("EmployeeId,StartDate")] Payroll payroll)
        {
            if (ModelState.IsValid)
            {
                payroll.PayrollID = Guid.NewGuid();
                _context.Add(payroll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName", payroll.EmployeeId);
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
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName", payroll.EmployeeId);
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
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName", payroll.EmployeeId);
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
