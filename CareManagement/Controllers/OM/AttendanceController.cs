/*using CareManagement.Data;
using CareManagement.Models.OM;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareManagement.Controllers.OM
{
    public class AttendanceController : Controller
    {
        private readonly CareManagementContext _context;

        public AttendanceController(CareManagementContext context)
        {
            _context = context;
        }

        // GET: Attendance/CheckInOut
        public IActionResult CheckInOut()
        {
            var currentEmployee = GetCurrentEmployee();
            if (currentEmployee == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if the employee has already checked in today
            var today = DateTime.Today;
            var checkIn = _context.Attendance.FirstOrDefault(a => a.EmployeeId == currentEmployee.EmployeeId && a.CheckIn.Date == today);

            // Determine whether the employee is currently checked in or checked out
            var isCheckedIn = checkIn != null && checkIn.CheckOut == null;

            var model = new CheckInOutViewModel
            {
                EmployeeId = currentEmployee.EmployeeId,
                IsCheckedIn = isCheckedIn
            };

            return View(model);
        }

        // POST: Attendance/CheckInOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckInOut(CheckInOutViewModel model)
        {
            var currentEmployee = GetCurrentEmployee();
            if (currentEmployee == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Check if the employee has already checked in today
                var today = DateTime.Today;
                var checkIn = _context.Attendance.FirstOrDefault(a => a.EmployeeId == currentEmployee.EmployeeId && a.CheckIn.Date == today);

                if (model.IsCheckedIn)
                {
                    if (checkIn == null)
                    {
                        // Employee is checking in for the first time today
                        var attendance = new Attendance
                        {
                            EmployeeId = currentEmployee.EmployeeId,
                            CheckIn = DateTime.Now
                        };
                        _context.Attendance.Add(attendance);
                        await _context.SaveChangesAsync();
                    }
                    else if (checkIn.CheckOut == null)
                    {
                        // Employee has already checked in today
                        ModelState.AddModelError("IsCheckedIn", "You are already checked in.");
                    }
                    else
                    {
                        // Employee has checked out earlier today and is now checking back in
                        checkIn.CheckOut = null;
                        checkIn.CheckIn = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    if (checkIn == null)
                    {
                        // Employee has not checked in yet today
                        ModelState.AddModelError("IsCheckedIn", "You need to check in first.");
                    }
                    else if (checkIn.CheckOut != null)
                    {
                        // Employee has already checked out today
                        ModelState.AddModelError("IsCheckedIn", "You have already checked out.");
                    }
                    else
                    {
                        // Employee is checking out for the day
                        checkIn.CheckOut = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return View(model);
        }

        private Employee GetCurrentEmployee()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _context.Employee.FirstOrDefault(e => e.UserId == userId);
        }
    }

}
*/