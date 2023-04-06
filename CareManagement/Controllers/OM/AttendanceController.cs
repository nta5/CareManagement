using CareManagement.Data;
using CareManagement.Models.OM;
using CareManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace CareManagement.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly CareManagementContext _context;

        public AttendanceController(CareManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(AttendanceViewModel model)
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName");
            var employees = await _context.Employee.ToListAsync();

            if (model == null)
            {
                model = new AttendanceViewModel
                {
                    EmployeeList = new SelectList(employees, "Id", "FirstName"),
                    IsCheckedIn = false
                };
            }
            else
            {
                // Check if the selected employee is currently checked in
                var currentShift = await _context.Shift
                    .Where(s => s.EmployeeId == model.EmployeeId && s.StartTime.Date == DateTime.Now.Date)
                    .OrderByDescending(s => s.StartTime)
                    .FirstOrDefaultAsync();

                model.IsCheckedIn = (currentShift != null && currentShift.EndTime == DateTime.MinValue);
            }

            // Get the TempData message and assign it to ViewBag
            ViewBag.Message = TempData["Message"]?.ToString();

            return View(model);
        }

        /* public async Task<IActionResult> Index(AttendanceViewModel model)
         {
             ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName");
             var employees = await _context.Employee.ToListAsync();
             if (model == null)
             {
                 model = new AttendanceViewModel
                 {
                     EmployeeList = new SelectList(employees, "Id", "FirstName"),
                     IsCheckedIn = false
                 };
             }


             // Get the TempData message and assign it to ViewBag
             ViewBag.Message = TempData["Message"]?.ToString();

             return View(model);
         }*/

        [HttpPost]
        public IActionResult CheckIn(AttendanceViewModel model)
        {
            model.currentAttendance = new Attendance();
            model.currentAttendance.CheckInTime = DateTime.Now;
            model.IsCheckedIn = true;
            // Create a new Shift object
            var shift = new Shift
            {
                EmployeeId = model.EmployeeId,
                ManagerId = model.EmployeeId,
                StartTime = DateTime.Now,
                EndTime = DateTime.MinValue,
                Sick = false,
                ScheduleId = model.EmployeeId
            };

            // Add the new Shift to the database
            _context.Add(shift);
            _context.SaveChanges();

            // Set the TempData message
            TempData["Message"] = $"You have checked in at {DateTime.Now.ToString("hh:mm tt")}.";

            return RedirectToAction("Index", model);
        }

        [HttpPost]
        public IActionResult CheckOut(AttendanceViewModel model)
        {
            model.IsCheckedIn = false;
            // Find the employee's current shift
            var shift = _context.Shift
                .Where(s => s.EmployeeId == model.EmployeeId && s.StartTime.Date == DateTime.Now.Date)
                .OrderByDescending(s => Math.Abs((s.StartTime - DateTime.Now).TotalMinutes))
                .LastOrDefault();

            if (shift != null)
            {
                // Update the end time of the current shift
                shift.EndTime = DateTime.Now;
                _context.SaveChanges();

                // Set the TempData message
                TempData["Message"] = $"You have checked out at {DateTime.Now.ToString("hh:mm tt")}.";
            }

            // Redirect to the Index action
            return RedirectToAction("Index", model);
        }
    }
}
