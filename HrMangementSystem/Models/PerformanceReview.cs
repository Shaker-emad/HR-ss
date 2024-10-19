using HrManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class PerformanceReview
    {
        [Key]
        public int ReviewID { get; set; }

        // Foreign Key
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime ReviewDate { get; set; }
        public string ReviewDescription { get; set; }

        // Foreign Key
        public int ReviewerID { get; set; }
        public Manager Reviewer { get; set; }

        public decimal Rating { get; set; }
    }
}
