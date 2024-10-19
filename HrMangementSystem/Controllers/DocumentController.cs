using HrMangementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HrMangementSystem.Controllers
{
    public class DocumentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DocumentController(AppDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult ManageDocuments()
        {
            // استرجاع ملفات الشركة من مجلد wwwroot/Documents
            var companyDocuments = new List<Document>();
            var documentsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Documents");
            if (Directory.Exists(documentsFolder))
            {
                var files = Directory.GetFiles(documentsFolder);
                foreach (var file in files)
                {
                    companyDocuments.Add(new Document
                    {
                        FileName = Path.GetFileName(file),
                        FilePath = file,
                        DocumentType = "Company Document",
                        UploadDate = System.IO.File.GetLastWriteTime(file)
                    });
                }
            }

            // استرجاع بيانات الموظفين من قاعدة البيانات
            var staffFiles = _context.Employees.ToList();

            // استرجاع ملفات الموظفين
            var employeeFiles = _context.Employees
                .Select(e => new
                {
                    EmployeeId = e.EmployeeId,
                    FullName = $"{e.FirstName} {e.LastName}",
                    Documents = new List<Document>
                    {
                new Document { DocumentType = "Certificate", FileName = e.Certificate, FilePath = e.Certificate },
                new Document { DocumentType = "Resume", FileName = e.Resume, FilePath = e.Resume },
                new Document { DocumentType = "Image", FileName = e.Image, FilePath = e.Image }
                    }
                })
                .ToList()
                .GroupBy(e => e.EmployeeId)
                .ToDictionary(
                    g => g.Key.ToString(),
                    g => g.SelectMany(f => f.Documents.Where(doc => !string.IsNullOrEmpty(doc.FileName))).ToList()
                );

            // إنشاء النموذج لعرضه في الـ View
            var model = new Tuple<List<Document>, IEnumerable<Employee>, Dictionary<string, List<Document>>, Dictionary<string, int>>(
                companyDocuments,
                staffFiles,
                employeeFiles,
                employeeFiles.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count)
            );

            return View(model);
        }




        public async Task<IActionResult> DeleteDocument(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            // احصل على مسار الملف من ملف الوثيقة
            var documentsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Documents");
            var filePath = Path.Combine(documentsFolder, document.FileName);

            // تحقق مما إذا كان الملف موجودًا في النظام
            if (System.IO.File.Exists(filePath))
            {
                // حذف الملف من النظام
                System.IO.File.Delete(filePath);
            }

            // إزالة الوثيقة من قاعدة البيانات
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageDocuments");
        }


        public async Task<IActionResult> DeleteEmployeeFile(int employeeId, string fileType)
        {
            // ابحث عن الموظف باستخدام معرفه
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            // مسار مجلد الملفات
            var documentsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Documents");

            // تحقق من نوع الملف الذي تريد حذفه (صورة أو شهادة أو سيرة ذاتية)
            string filePath = "";
            if (fileType == "Image" && !string.IsNullOrEmpty(employee.Image))
            {
                filePath = Path.Combine(documentsFolder, employee.Image);
                employee.Image = null;  // احذف اسم الملف من قاعدة البيانات
            }
            else if (fileType == "Certificate" && !string.IsNullOrEmpty(employee.Certificate))
            {
                filePath = Path.Combine(documentsFolder, employee.Certificate);
                employee.Certificate = null;  // احذف اسم الملف من قاعدة البيانات
            }
            else if (fileType == "Resume" && !string.IsNullOrEmpty(employee.Resume))
            {
                filePath = Path.Combine(documentsFolder, employee.Resume);
                employee.Resume = null;  // احذف اسم الملف من قاعدة البيانات
            }
            else
            {
                return BadRequest("Invalid file type or no file exists.");
            }

            // تحقق مما إذا كان الملف موجودًا في النظام
            if (System.IO.File.Exists(filePath))
            {
                // حذف الملف من نظام الملفات
                System.IO.File.Delete(filePath);
            }

            // حفظ التغييرات في قاعدة البيانات
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageDocuments");
        }


        [HttpPost]
        public async Task<IActionResult> UploadCompanyDocument(IFormFile companyFile)
        {
            if (companyFile == null || companyFile.Length == 0)
            {
                ModelState.AddModelError("File", "The file is empty or not provided.");
                return await ReloadManageDocumentsView();
            }

            // Check if Documents folder exists within wwwroot
            var documentsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Documents");
            if (!Directory.Exists(documentsFolder))
            {
                Directory.CreateDirectory(documentsFolder); // Create folder if not exists
            }

            var filePath = Path.Combine(documentsFolder, companyFile.FileName);

            try
            {
                // Copy file to the specified path using a stream
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await companyFile.CopyToAsync(stream);
                }

                // Create Document object and save data in database
                var document = new Document
                {
                    FileName = companyFile.FileName,
                    FilePath = filePath,
                    DocumentType = "Company Document",
                    UploadDate = DateTime.Now
                };

                _context.Documents.Add(document);
                await _context.SaveChangesAsync();

                return RedirectToAction("ManageDocuments");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("File", "File upload failed: " + ex.Message);
                return await ReloadManageDocumentsView();
            }
        }

        private async Task<IActionResult> ReloadManageDocumentsView()
        {
            var companyDocuments = _context.Documents.ToList();
            var staffFiles = _context.Employees.ToList();

            var employeeFiles = _context.Employees
                .Select(e => new
                {
                    EmployeeId = e.EmployeeId,
                    FullName = $"{e.FirstName} {e.LastName}",
                    Documents = new List<Document>
                    {
                        new Document { DocumentType = "Certificate", FileName = e.Certificate, FilePath = e.Certificate },
                        new Document { DocumentType = "Resume", FileName = e.Resume, FilePath = e.Resume },
                        new Document { DocumentType = "Image", FileName = e.Image, FilePath = e.Image }
                    }
                })
                .ToList()
                .GroupBy(e => e.EmployeeId)
                .ToDictionary(g => g.Key.ToString(), g => g.SelectMany(f => f.Documents.Where(doc => !string.IsNullOrEmpty(doc.FileName))).ToList());

            var model = new Tuple<List<Document>, IEnumerable<Employee>, Dictionary<string, List<Document>>, Dictionary<string, int>>(
                companyDocuments,
                staffFiles,
                employeeFiles,
                employeeFiles.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count)
            );

            return View("ManageDocuments", model);
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            // تحقق من أن اسم الملف غير فارغ وأنه لا يحتوي على أحرف غير صالحة
            if (string.IsNullOrEmpty(fileName) || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                return BadRequest("Invalid file name provided.");
            }

            // تكوين مسار الملف داخل مجلد wwwroot/Documents
            var documentsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Documents");
            var filePath = Path.Combine(documentsFolder, fileName);

            // تحقق مما إذا كان الملف موجودًا
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    // حذف الملف من النظام
                    System.IO.File.Delete(filePath);
                    return RedirectToAction("ManageDocuments");
                }
                catch (IOException ex)
                {
                    // إذا حدث خطأ أثناء حذف الملف
                    return StatusCode(500, $"Error deleting file: {ex.Message}");
                }
            }
            else
            {
                return NotFound("File not found.");
            }
        }
        // Action method لتحميل ملف من الموظفين
        public async Task<IActionResult> DownloadEmployeeFile(int id)
        {
            // احصل على الموظف باستخدام id
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            // احصل على نوع الملف للتحميل (يمكنك استخدام شرط if لتحديد النوع)
            string filePath = string.Empty;
            string fileName = string.Empty;

            if (!string.IsNullOrEmpty(employee.Image))
            {
                filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Documents", employee.Image);
                fileName = "EmployeeImage_" + employee.FirstName + employee.LastName + Path.GetExtension(filePath);
            }
            else if (!string.IsNullOrEmpty(employee.Certificate))
            {
                filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Documents", employee.Certificate);
                fileName = "EmployeeCertificate_" + employee.FirstName + employee.LastName + Path.GetExtension(filePath);
            }
            else if (!string.IsNullOrEmpty(employee.Resume))
            {
                filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Documents", employee.Resume);
                fileName = "EmployeeResume_" + employee.FirstName + employee.LastName + Path.GetExtension(filePath);
            }

            // تحقق مما إذا كان الملف موجودًا
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // قم بتنزيل الملف
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), fileName);
        }

        // دالة للحصول على نوع الملف
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        // قائمة الأنواع الملفات
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".doc", "application/vnd.ms-word" },
                { ".docx", "application/vnd.ms-word" }
            };
        }
    }
}




