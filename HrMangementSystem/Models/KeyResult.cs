using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class KeyResult
    {
        [Key]
        public int KeyResultID { get; set; }

        // Foreign Key
        public int ObjectiveID { get; set; }
        public Objective Objective { get; set; }

        public string KeyResultDescription { get; set; }
        public decimal TargetValue { get; set; }
        public decimal AchievedValue { get; set; }
        public string Status { get; set; }
    }
}
