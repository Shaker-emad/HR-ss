using HrManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrMangementSystem.Models
{
    public class PerformanceProgress
    {
        [Key]
        public int ProgressID { get; set; }

        // Foreign Key
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }


        [ForeignKey("Objective")]
        public int ObjectiveID { get; set; }
        public virtual Objective Objective { get; set; }

        [Required]
        [Range(0, 100)]
        public int Progress { get; set; }

        [Required]
        public string ProgressDescription { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ProgressDate { get; set; }

        [Required]
        public string Status { get; set; }







    }

}
