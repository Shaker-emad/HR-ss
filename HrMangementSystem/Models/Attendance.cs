using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceID { get; set; }

        // Foreign Key
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public DateTime Date { get; set; }
    }
}
