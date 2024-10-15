using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace HrMangementSystem.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; } // Primary Key

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        // Not mapped in DB, for image upload
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public string Image { get; set; } // Path to the image file
    }
}
