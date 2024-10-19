using HrMangementSystem.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HrManagementSystem.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceID { get; set; }

        [Required(ErrorMessage = "Employee is required")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }



        [Required(ErrorMessage = "Check-in time is required")]
        [Display(Name = "Check-in Time")]
        [DataType(DataType.Time)]
        public DateTime CheckInTime { get; set; }

        [Required(ErrorMessage = "Check-out time is required")]
        [Display(Name = "Check-out Time")]
        [DataType(DataType.Time)]
        public DateTime CheckOutTime { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Attendance Date")]
        public DateTime Date { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        // Custom validation method
        public bool IsValidTimeRange()
        {
            return CheckOutTime > CheckInTime;
        }
    }
}
