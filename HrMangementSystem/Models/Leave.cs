using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class Leave
    {
        [Key]
        public int LeaveID { get; set; }

        // Foreign Key
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public string LeaveType { get; set; }
        public string LeaveStatus { get; set; }
    }
}
