using HrManagementSystem.Models;
using HrMangementSystem.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic; // تأكد من استيراد هذا إذا كنت تستخدم ICollection
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace HrMangementSystem.Models
{
    public class Employee
    {
        // معرف الموظف
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(1000, 1000000, ErrorMessage = "Salary must be between 1000 and 1,000,000")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Hire Date is required")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }

        // علاقة مع قسم الموظف
        public Department Department { get; set; }

        // علاقة مع تقدم الأداء
        public ICollection<PerformanceProgress> PerformanceProgresses { get; set; }
        public ICollection<PerformanceReview> PerformanceReviews { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }

        // خصائص الملفات التي لا نريد تخزينها في قاعدة البيانات
        [NotMapped]
        public IFormFile? ImageFile { get; set; }  // لرفع صورة الموظف 

        [NotMapped]
        public IFormFile? CertificateFile { get; set; }  // لرفع الشهادات 

        [NotMapped]
        public IFormFile? ResumeFile { get; set; }  // لرفع السيرة الذاتية 

        // خصائص لتخزين أسماء الملفات في قاعدة البيانات
        public string? Image { get; set; }  // لتخزين اسم صورة الموظف
        public string? Certificate { get; set; } // Make it nullable
        public string? Resume { get; set; }  // لتخزين اسم السيرة الذاتية

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}