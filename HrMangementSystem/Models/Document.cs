using System.ComponentModel.DataAnnotations;

namespace HrMangementSystem.Models
{
    public class Document
    {
        [Key]
        public int DocumentID { get; set; }

        // Foreign Key
        public int ApplicantID { get; set; }
        public JobApplication JobApplication { get; set; }

        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int FileSize { get; set; }
        public DateTime UploadDate { get; set; }
        public byte[] CVFile { get; set; }
    }
}
