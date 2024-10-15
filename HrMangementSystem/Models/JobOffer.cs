using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class JobOffer
    {
        [Key]
        public int JobOfferID { get; set; }
        public string JobOfferDescription { get; set; }

        // Foreign Key
        public int ManagerID { get; set; }
        public Manager Manager { get; set; }
    }
}
