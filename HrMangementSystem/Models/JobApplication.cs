using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class JobApplication
    {
        [Key]
        public int ApplicationID { get; set; }
        public string ApplicantName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ApplicationStatus { get; set; }
        public string Education { get; set; }
        public int Age { get; set; }
        public string Skills { get; set; }
        public string Experience { get; set; }
        public string Address { get; set; }
        public int GraduationYear { get; set; }
        public string JobTitle { get; set; }
        public string Nationality { get; set; }
    }
}
