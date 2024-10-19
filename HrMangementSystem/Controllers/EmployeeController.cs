using HrMangementSystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HrMangementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();
            return View(employees);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            // معالجة تحميل الصورة
            if (employee.ImageFile != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                var extension = Path.GetExtension(employee.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                var path = Path.Combine("wwwroot/Image/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await employee.ImageFile.CopyToAsync(fileStream);
                }

                employee.Image = fileName;
            }

            // معالجة تحميل الشهادة
            if (employee.CertificateFile != null)
            {
                var certificateName = Path.GetFileNameWithoutExtension(employee.CertificateFile.FileName);
                var certificateExtension = Path.GetExtension(employee.CertificateFile.FileName);
                certificateName = certificateName + DateTime.Now.ToString("yymmssfff") + certificateExtension;
                var certificatePath = Path.Combine("wwwroot/Certificates/", certificateName);

                using (var fileStream = new FileStream(certificatePath, FileMode.Create))
                {
                    await employee.CertificateFile.CopyToAsync(fileStream);
                }

                // تخزين اسم الشهادة
                employee.Certificate = certificateName;
            }

            // معالجة تحميل السيرة الذاتية
            if (employee.ResumeFile != null)
            {
                var resumeName = Path.GetFileNameWithoutExtension(employee.ResumeFile.FileName);
                var resumeExtension = Path.GetExtension(employee.ResumeFile.FileName);
                resumeName = resumeName + DateTime.Now.ToString("yymmssfff") + resumeExtension;
                var resumePath = Path.Combine("wwwroot/Resumes/", resumeName);

                using (var fileStream = new FileStream(resumePath, FileMode.Create))
                {
                    await employee.ResumeFile.CopyToAsync(fileStream);
                }

                // تخزين اسم السيرة الذاتية
                employee.Resume = resumeName;
            }

            // إضافة الموظف إلى قاعدة البيانات
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // إعادة التوجيه إلى صفحة الفهرس بعد الحفظ
            return RedirectToAction(nameof(Index));
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            // Populate departments for the dropdown
            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee updatedEmployee)
        {
            if (id != updatedEmployee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingEmployee = await _context.Employees.FindAsync(updatedEmployee.EmployeeId);
                if (existingEmployee == null)
                {
                    return NotFound();
                }

                // Update only the specified fields
                existingEmployee.FirstName = updatedEmployee.FirstName;
                existingEmployee.LastName = updatedEmployee.LastName;
                existingEmployee.Position = updatedEmployee.Position;
                existingEmployee.Salary = updatedEmployee.Salary;
                existingEmployee.DepartmentId = updatedEmployee.DepartmentId;

                // Save changes
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If the model state is invalid, repopulate the ViewBag
            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", updatedEmployee.DepartmentId);

            return View(updatedEmployee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department) // Optional: Include related data if needed
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
