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
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
    }
}
