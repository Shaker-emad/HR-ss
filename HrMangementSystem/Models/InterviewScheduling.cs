using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class InterviewScheduling
    {
        [Key]
        public int InterviewID { get; set; }

        // Foreign Key
        public int ApplicationID { get; set; }
        public JobApplication JobApplication { get; set; }

        public DateTime InterviewDate { get; set; }
        public string InterviewerName { get; set; }
        public string InterviewLocation { get; set; }
        public string InterviewStatus { get; set; }
    }
}
