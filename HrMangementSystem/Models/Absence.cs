
using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class Absence
    {
        [Key]
        public int AbsenceID { get; set; }

        // Foreign Key
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}
