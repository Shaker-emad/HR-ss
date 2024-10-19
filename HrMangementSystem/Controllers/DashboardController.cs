using HrMangementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // تأكد من أن هذا موجود

namespace HrManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // إحصائيات الحضور والغياب
            var attendanceStats = await _context.Attendances
                .Where(a => a.CheckInTime != null) // تأكد من أن CheckInTime ليست NULL
                .GroupBy(a => a.Date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    AttendanceRate = g.Count() * 100.0 / (g.Count() == 0 ? 1 : g.Count()), // تجنب القسمة على 0
                    AbsenceRate = _context.Attendances.Count(a => a.Date.Month == g.Key && a.CheckInTime == null) * 100.0 / (g.Count() == 0 ? 1 : g.Count()) // تأكد من حساب الغيابات بشكل صحيح
                })
                .ToListAsync();


            // موظف الشهر
            // موظف الشهر
            var employeeOfTheMonth = await _context.Employees
                .Select(e => new
                {
                    Employee = e,
                    PerformanceScore = e.PerformanceProgresses.Sum(p => p.Progress) // استبدل Score بالخاصية المناسبة في PerformanceProgress
                })
                .OrderByDescending(e => e.PerformanceScore)
                .FirstOrDefaultAsync();


            return View();
        }

    }
}
