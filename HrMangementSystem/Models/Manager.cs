using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class Manager
    {
        [Key]
        public int ManagerId { get; set; } // Primary Key
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Foreign Key
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public ICollection<PerformanceReview> PerformanceReviews { get; set; }
    }
}
