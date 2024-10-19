using HrManagementSystem.Models;
using HrMangementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HrMangementSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly AppDbContext _context;

        public AttendanceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Attendance
        public async Task<IActionResult> Index()
        {
            var attendances = await _context.Attendances
                .Include(a => a.Employee)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
            return View(attendances);
        }

        // GET: Attendance/Create
        public IActionResult Create()
        {
            PrepareEmployeesList();
            var attendance = new Attendance
            {
                Date = DateTime.Today,
                CheckInTime = DateTime.Now,
                CheckOutTime = DateTime.Now // Don't set a default checkout time
            };

            return View(attendance);
        }


        /*[ValidateAntiForgeryToken]*/
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create(Attendance attendance)
        {
            // Validate CheckOutTime
            if (attendance.CheckOutTime != null && attendance.CheckOutTime <= attendance.CheckInTime)
            {
                ModelState.AddModelError("CheckOutTime", "Check-out time must be after check-in time.");
            }

            // Validate Date
            if (attendance.Date.Date > DateTime.Today)
            {
                ModelState.AddModelError("Date", "Attendance date cannot be in the future.");
            }

            if (!ModelState.IsValid)
            {
                PrepareEmployeesList();
                return View(attendance);
            }

            // Check for existing attendance
            var existingAttendance = await _context.Attendances
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.EmployeeId == attendance.EmployeeId && a.Date.Date == attendance.Date.Date);

            if (existingAttendance != null)
            {
                ModelState.AddModelError(string.Empty, "Attendance record already exists for this employee on this date.");
                PrepareEmployeesList();
                return View(attendance);
            }

            try
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Attendance record created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while saving the attendance record: {ex.Message}");
                PrepareEmployeesList();
                return View(attendance);
            }
        }






        // GET: Attendance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.AttendanceID == id);

            if (attendance == null)
            {
                return NotFound();
            }

            PrepareEmployeesList();
            return View(attendance);
        }

        // POST: Attendance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttendanceID,EmployeeID,CheckInTime,CheckOutTime,Date")] Attendance attendance)
        {
            if (id != attendance.AttendanceID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    // Validate check-in and check-out times
                    if (attendance.CheckOutTime <= attendance.CheckInTime)
                    {
                        ModelState.AddModelError("CheckOutTime", "Check-out time must be after check-in time");
                        PrepareEmployeesList();
                        return View(attendance);
                    }

                    // Validate date is not in the future
                    if (attendance.Date.Date > DateTime.Today)
                    {
                        ModelState.AddModelError("Date", "Attendance date cannot be in the future");
                        PrepareEmployeesList();
                        return View(attendance);
                    }

                    // Check for existing attendance (excluding current record)
                    var existingAttendance = await _context.Attendances
                        .AsNoTracking()
                        .FirstOrDefaultAsync(a => a.EmployeeId == attendance.EmployeeId
                                             && a.Date.Date == attendance.Date.Date
                                             && a.AttendanceID != attendance.AttendanceID);

                    if (existingAttendance != null)
                    {
                        ModelState.AddModelError(string.Empty, "Another attendance record already exists for this employee on this date");
                        PrepareEmployeesList();
                        return View(attendance);
                    }

                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Attendance record updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.AttendanceID))
                    {
                        return NotFound();
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    // Log the exception here
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the attendance record.");
                }
            }

            PrepareEmployeesList();
            return View(attendance);
        }

        // GET: Attendance/Delete/5
        [HttpPost]
        public IActionResult Delete(int AttendanceID)
        {
            // تحقق من وجود السجل
            var attendance = _context.Attendances.Find(AttendanceID);
            if (attendance == null)
            {
                return NotFound();
            }

            // احذف السجل
            _context.Attendances.Remove(attendance);
            _context.SaveChanges();

            // إعادة التوجيه بعد الحذف
            return RedirectToAction("Index"); // أو أي مكان تريد إعادة توجيه المستخدم إليه
        }


        // POST: Attendance/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);

            if (attendance == null)
            {
                return NotFound();
            }

            try
            {
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Attendance record deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while deleting the attendance record: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }


        private void PrepareEmployeesList()
        {
            ViewBag.Employees = new SelectList(_context.Employees
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    e.EmployeeId,
                    FullName = $"{e.FirstName} {e.LastName}"
                }), "EmployeeId", "FullName");
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.AttendanceID == id);
        }
    }
}