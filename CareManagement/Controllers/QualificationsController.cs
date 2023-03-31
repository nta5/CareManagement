using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareManagement.Data;
using CareManagement.Models.SCHDL;

namespace CareManagement.Controllers
{
    public class QualificationsController : Controller
    {
        private readonly CareManagementContext _context;

        public QualificationsController(CareManagementContext context)
        {
            _context = context;
        }

        // GET: Qualifications
        public async Task<IActionResult> Index()
        {
              return _context.Qualification != null ? 
                          View(await _context.Qualification.ToListAsync()) :
                          Problem("Entity set 'CareManagementContext.Qualification'  is null.");
        }

        // GET: Qualifications/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Qualification == null)
            {
                return NotFound();
            }

            var qualification = await _context.Qualification
                .FirstOrDefaultAsync(m => m.QualificationId == id);
            if (qualification == null)
            {
                return NotFound();
            }

            return View(qualification);
        }

        // GET: Qualifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Qualifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QualificationId,QualificationDescription")] Qualification qualification)
        {
            if (ModelState.IsValid)
            {
                qualification.QualificationId = Guid.NewGuid();
                _context.Add(qualification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(qualification);
        }

        // GET: Qualifications/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Qualification == null)
            {
                return NotFound();
            }

            var qualification = await _context.Qualification.FindAsync(id);
            if (qualification == null)
            {
                return NotFound();
            }
            return View(qualification);
        }

        // POST: Qualifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("QualificationId,QualificationDescription")] Qualification qualification)
        {
            if (id != qualification.QualificationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qualification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QualificationExists(qualification.QualificationId))
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
            return View(qualification);
        }

        // GET: Qualifications/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Qualification == null)
            {
                return NotFound();
            }

            var qualification = await _context.Qualification
                .FirstOrDefaultAsync(m => m.QualificationId == id);
            if (qualification == null)
            {
                return NotFound();
            }

            return View(qualification);
        }

        // POST: Qualifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Qualification == null)
            {
                return Problem("Entity set 'CareManagementContext.Qualification'  is null.");
            }
            var qualification = await _context.Qualification.FindAsync(id);
            if (qualification != null)
            {
                _context.Qualification.Remove(qualification);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QualificationExists(Guid id)
        {
          return (_context.Qualification?.Any(e => e.QualificationId == id)).GetValueOrDefault();
        }
    }
}
