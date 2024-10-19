using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; } // Primary Key
        public string DepartmentName { get; set; }
        public string Location { get; set; }

        //Foregin Key
        public int ManagerId { get; set; }
        public Manager Manager { get; set; }

        // Navigation property for employees in the department
        public ICollection<Objective> Objectives { get; set; } // العلاقة بين الأقسام والأهداف

        public ICollection<Employee> Employees { get; set; }
    }
}
