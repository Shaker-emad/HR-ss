using HrMangementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HrMangementSystem.Controllers
{
    public class PerformanceController : Controller
    {
        private readonly AppDbContext _context;

        public PerformanceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Performance
        public async Task<IActionResult> Index()
        {
            var performance = await _context.PerformanceProgresses
                .Include(pp => pp.Employee)
                .Include(pp => pp.Objective)
                .ToListAsync();
            return View(performance);
        }

        // GET: Performance/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        // POST: Performance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerformanceProgress performanceProgress)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Add(performanceProgress);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

            await PopulateDropdownsAsync(performanceProgress);
            return View(performanceProgress);
        }

        // GET: Performance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performanceProgress = await _context.PerformanceProgresses
                .FirstOrDefaultAsync(p => p.ProgressID == id);

            if (performanceProgress == null)
            {
                return NotFound();
            }

            await PopulateDropdownsAsync(performanceProgress);
            return View(performanceProgress);
        }

        // POST: Performance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PerformanceProgress performanceProgress)
        {
            if (id != performanceProgress.ProgressID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(performanceProgress);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformanceProgressExists(performanceProgress.ProgressID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

            await PopulateDropdownsAsync(performanceProgress);
            return View(performanceProgress);
        }

        // GET: Performance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performanceProgress = await _context.PerformanceProgresses
                .Include(p => p.Employee)
                .Include(p => p.Objective)
                .FirstOrDefaultAsync(p => p.ProgressID == id);

            if (performanceProgress == null)
            {
                return NotFound();
            }

            return View(performanceProgress);
        }

        // POST: Performance/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var performanceProgress = await _context.PerformanceProgresses.FindAsync(id);
            if (performanceProgress == null)
            {
                return NotFound();
            }

            try
            {
                _context.PerformanceProgresses.Remove(performanceProgress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while deleting: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }


        // Helper method to check if PerformanceProgress exists
        private bool PerformanceProgressExists(int id)
        {
            return _context.PerformanceProgresses.Any(e => e.ProgressID == id);
        }

        // Helper method to populate dropdown lists for Employee and Objective
        private async Task PopulateDropdownsAsync(PerformanceProgress performanceProgress = null)
        {
            ViewBag.EmployeeId = new SelectList(
                await _context.Employees
                    .Select(e => new { e.EmployeeId, FullName = $"{e.FirstName} {e.LastName}" })
                    .ToListAsync(),
                "EmployeeId", "FullName", performanceProgress?.EmployeeId);

            ViewBag.ObjectiveID = new SelectList(
                await _context.Objectives
                    .Select(o => new { o.ObjectiveID, o.ObjectiveDescription })
                    .ToListAsync(),
                "ObjectiveID", "ObjectiveDescription", performanceProgress?.ObjectiveID);
        }
    }
}
