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
using CareManagement.Models.OM;
using System.ComponentModel.DataAnnotations;

namespace CareManagement.Controllers.SCHDL
{
    public class SchedulesController : Controller
    {
        private readonly CareManagementContext _context;

        public SchedulesController(CareManagementContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            //var careManagementContext = _context.Schedule.Include(s => s.Renter).Include(s => s.Service).Include(s => s.Shift);
            //return View(await careManagementContext.ToListAsync());

            var careManagementContext = _context.Schedule
                .Include(s => s.Renter)
                .Include(s => s.Service)
                .Include(s => s.Shift)
                .Select(s => new ScheduleViewModel
                {
                    Service = s.Service,
                    Renter = s.Renter,
                    Shift = s.Shift,
                    ScheduleId = s.ScheduleId,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsInvoiced = s.IsInvoiced,
                    IsRepeating = s.IsRepeating,
                    RepeatStartDate = s.RepeatStartDate,
                    RepeatEndDate = s.RepeatEndDate,
                    RenterId = s.RenterId,
                    ShiftID = s.ShiftID,
                    ServiceId = s.ServiceId,
                    EmployeeName = _context.Employee
                        .Where(e => e.EmployeeId == s.Shift.EmployeeId)
                        .Select(e => $"{e.FirstName} {e.LastName}")
                        .FirstOrDefault()
                }); 

                return View(await careManagementContext.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .Include(s => s.Renter)
                .Include(s => s.Service)
                .Include(s => s.Shift)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {

            ViewData["RenterId"] = _context.Renter.Select(r => new SelectListItem
            {
                Value = r.RenterId.ToString(),
                Text = $"{r.Name} ({r.RmNumber})"
            }).ToList();

            ViewData["ServiceId"] = new SelectList(_context.Service, "ServiceId", "Type");

            ViewData["ShiftID"] = _context.Shift
                .Join(_context.Employee,
                    shift => shift.EmployeeId,
                    employee => employee.EmployeeId,
                    (shift, employee) => new SelectListItem
                    {
                        Value = shift.ShiftId.ToString(),
                        Text = $"{employee.FirstName} {employee.LastName}"
                    })
                .ToList();

            ViewData["Hours"] = _context.Service.Select(s => new SelectListItem
            {
                Value = s.ServiceId.ToString(),
                Text = s.Hours.ToString()
            }).ToList();

            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleId,StartTime,EndTime,IsInvoiced,IsRepeating,RepeatStartDate,RepeatEndDate,RenterId,ShiftID,ServiceId")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                //schedule.ScheduleId = Guid.NewGuid();
                //_context.Add(schedule);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));

                if (schedule.IsRepeating && schedule.RepeatStartDate.HasValue && schedule.RepeatEndDate.HasValue)
                {
                    DateTime currentStartTime = schedule.StartTime;
                    //DateTime currentEndTime = schedule.EndTime.Date + (schedule.RepeatStartDate.Value.TimeOfDay - schedule.StartTime.TimeOfDay);
                    DateTime currentEndTime = schedule.EndTime;
                    while (currentStartTime <= schedule.RepeatEndDate.Value)
                    {
                        Schedule newSchedule = new Schedule
                        {
                            ScheduleId = Guid.NewGuid(),
                            StartTime = currentStartTime,
                            EndTime = currentEndTime,
                            IsInvoiced = schedule.IsInvoiced,
                            IsRepeating = schedule.IsRepeating,
                            RepeatStartDate = schedule.RepeatStartDate,
                            RepeatEndDate = schedule.RepeatEndDate,
                            RenterId = schedule.RenterId,
                            ShiftID = schedule.ShiftID,
                            ServiceId = schedule.ServiceId
                        };
                        _context.Add(newSchedule);
                        await _context.SaveChangesAsync();

                        // Move to the next week
                        currentStartTime = currentStartTime.AddDays(7);
                        currentEndTime = currentEndTime.AddDays(7);
                    }
                }
                else
                {
                    _context.Add(schedule);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RenterId"] = new SelectList(_context.Renter, "RenterId", "RenterId", schedule.RenterId);
            ViewData["ServiceId"] = new SelectList(_context.Service, "ServiceId", "Type", schedule.ServiceId);
            ViewData["ShiftID"] = new SelectList(_context.Shift, "ShiftId", "ShiftId", schedule.ShiftID);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            ViewData["RenterId"] = _context.Renter.Select(r => new SelectListItem
            {
                Value = r.RenterId.ToString(),
                Text = $"{r.Name} ({r.RmNumber})"
            }).ToList();
            // Set the selected value for the SelectList
            ((List<SelectListItem>)ViewData["RenterId"]).FirstOrDefault(item => item.Value == schedule.RenterId.ToString()).Selected = true;

            ViewData["ServiceId"] = new SelectList(_context.Service, "ServiceId", "Type", schedule.ServiceId);

            ViewData["ShiftID"] = _context.Shift
            .Join(_context.Employee,
                shift => shift.EmployeeId,
                employee => employee.EmployeeId,
                (shift, employee) => new SelectListItem
                {
                    Value = shift.ShiftId.ToString(),
                    Text = $"{employee.FirstName} {employee.LastName}"
                })
            .ToList();
            ((List<SelectListItem>)ViewData["ShiftID"]).FirstOrDefault(item => item.Value == schedule.ShiftID.ToString()).Selected = true;

            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ScheduleId,StartTime,EndTime,IsInvoiced,IsRepeating,RepeatStartDate,RepeatEndDate,RenterId,ShiftID,ServiceId")] Schedule schedule)
        {
            if (id != schedule.ScheduleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.ScheduleId))
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
            ViewData["RenterId"] = new SelectList(_context.Renter, "RenterId", "RenterId", schedule.RenterId);
            ViewData["ServiceId"] = new SelectList(_context.Service, "ServiceId", "Type", schedule.ServiceId);
            ViewData["ShiftID"] = new SelectList(_context.Shift, "ShiftId", "ShiftId", schedule.ShiftID);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .Include(s => s.Renter)
                .Include(s => s.Service)
                .Include(s => s.Shift)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Schedule == null)
            {
                return Problem("Entity set 'CareManagementContext.Schedule'  is null.");
            }
            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedule.Remove(schedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(Guid id)
        {
          return (_context.Schedule?.Any(e => e.ScheduleId == id)).GetValueOrDefault();
        }
    }

    internal class ScheduleViewModel
    {

        public Guid ScheduleId { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Invoiced")]
        public bool IsInvoiced { get; set; }

        [Display(Name = "Repeat")]
        public bool IsRepeating { get; set; }

        [Display(Name = "Repeat From")]
        public DateTime? RepeatStartDate { get; set; }

        [Display(Name = "Repeat Until")]
        public DateTime? RepeatEndDate { get; set; }

        public Guid RenterId { get; set; }
        public Guid ShiftID { get; set; }
        public Guid ServiceId { get; set; }

        [Display(Name = "Employee")]
        public string EmployeeName { get; set; }
        public Service? Service { get; internal set; }
        public Renter? Renter { get; internal set; }
        public Shift? Shift { get; internal set; }
    }

    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly CareManagementContext _context;

        public ApiController(CareManagementContext context)
        {
            _context = context;
        }

        [HttpGet("api/services/{id}")]
        public async Task<ActionResult<Service>> GetService(Guid id)
        {
            var service = await _context.Service.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }

        [HttpGet("api/shifts")]
        public async Task<IActionResult> GetAvailableShiftIds([FromQuery] Guid serviceId, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        { 
            var service= await _context.Service.FindAsync(serviceId);

            var availableShifts = await _context.Shift
                .Join
                (
                    _context.Employee,
                    shift => shift.EmployeeId,
                    employee => employee.EmployeeId,
                    (shift, employee) => new { shift, employee }
                )
                .GroupJoin
                (
                    _context.Schedule,
                    joined => joined.shift.ShiftId,
                    schedule => schedule.ShiftID,
                    (joined, schedules) => new { joined, schedules }
                )
                .Where
                (
                    grouped =>
                    grouped.joined.employee.QualificationId == service.QualificationId &&
                    grouped.joined.shift.StartTime <= startTime &&
                    grouped.joined.shift.EndTime >= endTime &&
                    (
                        !grouped.schedules.Any() ||
                        //TODO: add more conditions where schedule is before, after, overlap... 
                        grouped.schedules.All(schedule => schedule.EndTime <= startTime || schedule.StartTime >= endTime)
                    )
                )
                .Select(grouped => new
                {
                    ShiftId = grouped.joined.shift.ShiftId,
                    FirstName = grouped.joined.employee.FirstName,
                    LastName = grouped.joined.employee.LastName
                })
                .ToListAsync();

            return Ok(availableShifts);
        }
    }
}
