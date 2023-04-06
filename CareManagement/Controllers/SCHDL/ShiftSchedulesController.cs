using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CareManagement.Data;
using CareManagement.Models.SCHDL;
using CareManagement.Models.OM;
using CareManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using CareManagement.Models.AUTH;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CareManagement.Controllers.SCHDL
{
    [Authorize]
    public class ShiftSchedulesController : Controller
    {
        private readonly CareManagementContext _context;
        private UserManager<AppUser> userManager;

        public ShiftSchedulesController(CareManagementContext context, UserManager<AppUser> usrMgr)
        {
            _context = context;
            userManager = usrMgr;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var currentUser = await userManager.FindByIdAsync(userId);
            if (currentUser == null)
            {
                return RedirectToAction("AccessDenied");
            }
            var employeeInfo = await _context.Employee.FindAsync(currentUser.EmployeeId);

            if (employeeInfo == null)
            {
                return RedirectToAction("AccessDenied");
            }

            var viewModel = new ShiftSchedulesViewModel
            {
                SelectedEmployeeId = currentUser.EmployeeId,
                SelectedEmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName + " - " + employeeInfo.Title,
                StartDate = DateTime.Today
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind("SelectedEmployeeId,SelectedEmployeeName,StartDate")] ShiftSchedulesViewModel model)
        {
            model.DisplayedShift = await _context.Shift
                .Include(s => s.Schedules)
                .FirstOrDefaultAsync(s => s.EmployeeId == model.SelectedEmployeeId
                                    && s.StartTime.Date >= model.StartDate.Date
                                    && s.EndTime.Date <= model.StartDate.AddDays(6).Date);

            if (model.DisplayedShift != null)
            {
                model.DisplayedSchedules = await _context.Schedule
                    .Include(s => s.Service)
                    .Where(s => s.ShiftID == model.DisplayedShift.ShiftId)
                    .ToListAsync();
            }
            else
            {
                model.DisplayedSchedules = new List<Schedule>();
            }

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

