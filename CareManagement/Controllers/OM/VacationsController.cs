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
    public class VacationsController : Controller
    {
        private readonly CareManagementContext _context;

        public VacationsController(CareManagementContext context)
        {
            _context = context;
        }

        // GET: Vacations
        public async Task<IActionResult> Index()
        {
            var careManagementContext = _context.Vacation.Include(v => v.Employee);
            return View(await careManagementContext.ToListAsync());
        }

        // GET: Vacations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Vacation == null)
            {
                return NotFound();
            }

            var vacation = await _context.Vacation
                .Include(v => v.Employee)
                .FirstOrDefaultAsync(m => m.VacationId == id);
            if (vacation == null)
            {
                return NotFound();
            }

            return View(vacation);
        }

        // GET: Vacations/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName");
            return View();
        }

        // POST: Vacations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VacationId,EmployeeId,StartDate,EndDate,VacationRequest")] Vacation vacation)
        {
            if (ModelState.IsValid)
            {
                vacation.VacationId = Guid.NewGuid();
                _context.Add(vacation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName", vacation.EmployeeId);
            return View(vacation);
        }

        // GET: Vacations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Vacation == null)
            {
                return NotFound();
            }

            var vacation = await _context.Vacation.FindAsync(id);
            if (vacation == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName", vacation.EmployeeId);
            return View(vacation);
        }

        // POST: Vacations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VacationId,EmployeeId,StartDate,EndDate,VacationRequest")] Vacation vacation)
        {
            if (id != vacation.VacationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacationExists(vacation.VacationId))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "FirstNameLastName", vacation.EmployeeId);
            return View(vacation);
        }

        // GET: Vacations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Vacation == null)
            {
                return NotFound();
            }

            var vacation = await _context.Vacation
                .Include(v => v.Employee)
                .FirstOrDefaultAsync(m => m.VacationId == id);
            if (vacation == null)
            {
                return NotFound();
            }

            return View(vacation);
        }

        // POST: Vacations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Vacation == null)
            {
                return Problem("Entity set 'CareManagementContext.Vacation'  is null.");
            }
            var vacation = await _context.Vacation.FindAsync(id);
            if (vacation != null)
            {
                _context.Vacation.Remove(vacation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacationExists(Guid id)
        {
          return (_context.Vacation?.Any(e => e.VacationId == id)).GetValueOrDefault();
        }
    }
}
