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

        // GET: Attendance/CheckInOut
        public async Task<IActionResult> Index()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName");
            var employees = await _context.Employee.ToListAsync();
            var model = new AttendanceViewModel
            {
                EmployeeList = new SelectList(employees, "Id", "FirstName"),
                //IsCheckedIn= false
            };
            
            return View(model);
        }

        [HttpPost]
        public IActionResult CheckIn(AttendanceViewModel model)
        {
            model.currentAttendance = new Attendance();
            model.currentAttendance.CheckInTime = DateTime.Now;
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

            // Set the ViewBag message
            ViewBag.Message = $"You have checked in at {DateTime.Now.ToString("hh:mm tt")}.";

             return View("Index");
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

                // Set the ViewBag message
                ViewBag.Message = $"You have checked out at {DateTime.Now.ToString("hh:mm tt")}.";
            }

            // Redirect to the Index action
            return View("Index");
        }
    }
}
