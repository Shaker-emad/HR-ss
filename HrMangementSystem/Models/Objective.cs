using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class Objective
    {
        [Key]
        public int ObjectiveID { get; set; }
        public string ObjectiveName { get; set; }
        public string ObjectiveDescription { get; set; }
        public string ObjectiveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Foreign Key
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        // Optional Employee foreign key
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public ICollection<PerformanceProgress> PerformanceProgresses { get; set; }
    }
}
