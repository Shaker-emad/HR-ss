using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class PerformanceProgress
    {
        [Key]
        public int ProgressID { get; set; }

        // Foreign Key
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        // Foreign Key
        public int ObjectiveID { get; set; }
        public Objective Objective { get; set; }

        public string ProgressDescription { get; set; }
        public DateTime ProgressDate { get; set; }
        public string Status { get; set; }
    }

}
