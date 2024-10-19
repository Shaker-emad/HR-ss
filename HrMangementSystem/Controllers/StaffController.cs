using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore; // Ensure this is included for async methods
using HrMangementSystem.Models;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace HrMangementSystem.Controllers
{
    public class StaffController : Controller
    {
        private readonly AppDbContext _context; // Ensure you have the correct DbContext

        public StaffController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Staff/Login
        public IActionResult Login()
        {
            return View(); // Display the login page
        }

        // POST: Staff/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Staff staff)
        {
            return await Authenticate(staff.Email, staff.Password);
        }

        public async Task<IActionResult> Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Email and Password are required.";
                return View("Login"); // Make sure this refers to your login view
            }

            // Search for the user based on email and password
            var user = await _context.Staff
                .FirstOrDefaultAsync(s => s.Email == email && s.Password == password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("Login"); // Return to login view on failure
            }

            // Store user data in session
            HttpContext.Session.SetString("UserId", user.StaffId.ToString());
            HttpContext.Session.SetString("UserName", user.FirstName + " " + user.LastName);
            HttpContext.Session.SetString("UserRole", user.Role);

            // Redirect to a specific view that uses _Layout
            return RedirectToAction("Index", "Employee");
        }

        // Logout action
        public IActionResult Logout()
        {
            // Clear session data
            HttpContext.Session.Clear();
            return RedirectToAction("Login"); // Redirect to the login page
        }

        // GET: Staff/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        public async Task<IActionResult> Create(Staff staff)
        {
            if (ModelState.IsValid)
            {
                // Handle employee image upload if provided
                if (staff.ImageFile != null && staff.ImageFile.Length > 0) // Ensure there is an image
                {
                    var fileName = Path.GetFileNameWithoutExtension(staff.ImageFile.FileName);
                    var extension = Path.GetExtension(staff.ImageFile.FileName);
                    staff.Image = fileName + extension;

                    // Save image to the specified folder
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", staff.Image);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await staff.ImageFile.CopyToAsync(stream);
                    }
                }

                // Add staff member to the database
                _context.Staff.Add(staff);
                await _context.SaveChangesAsync();

                return RedirectToAction("ManageUsers"); // Redirect to the staff list
            }

            return View(staff); // If there are errors, return the model to the view
        }

        // GET: Staff/ManageUsers
        public IActionResult ManageUsers()
        {
            var staffList = _context.Staff.ToList();
            return View(staffList); // Return the staff list to the view
        }
    }
}
