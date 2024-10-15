using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HrMangementSystem.Models;
using HrMangementSystem.Controllers; // تأكد من تعديل هذا وفقًا لمكان قاعدة البيانات
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HrMangementSystem.Controllers
{
    public class StaffController : Controller
    {
        private readonly AppDbContext _context; // تأكد من أن لديك قاعدة بيانات محددة

        public StaffController(AppDbContext context)
        {
            _context = context;
        }

        // عرض صفحة إنشاء موظف جديد
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // معالجة بيانات إنشاء موظف جديد
        [HttpPost]
        public async Task<IActionResult> Create(Staff staff)
        {
            if (ModelState.IsValid)
            {
                // معالجة رفع صورة الموظف إذا تم رفعها
                if (staff.ImageFile != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(staff.ImageFile.FileName);
                    var extension = Path.GetExtension(staff.ImageFile.FileName);
                    staff.Image = fileName + extension;

                    // حفظ الصورة في المجلد المحدد
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", staff.Image);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await staff.ImageFile.CopyToAsync(stream);
                    }
                }

                // إضافة الموظف إلى قاعدة البيانات
                _context.Staff.Add(staff);
                await _context.SaveChangesAsync();

                return RedirectToAction("ManageUsers"); // إعادة التوجيه إلى قائمة الموظفين
            }

            return View(staff); // إذا كانت هناك أخطاء، إعادة عرض النموذج
        }

        // عرض قائمة الموظفين (تأكد من أن لديك هذا الإجراء)
        public IActionResult ManageUsers()
        {
            var staffList = _context.Staff.ToList();
            return View(staffList);
        }
    }
}
