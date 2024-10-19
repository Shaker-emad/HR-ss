using System.ComponentModel.DataAnnotations;
using HrMangementSystem;


namespace HrMangementSystem.Models;
public class Document
{
    [Key]
    public int DocumentID { get; set; }

    [Required]
    [StringLength(100)]
    public string DocumentType { get; set; }  // نوع الملف مثل "CV", "Certificate"

    [Required]
    [StringLength(255)]
    public string FileName { get; set; }  // اسم الملف

    [Required]
    [StringLength(100)]
    public string ContentType { get; set; }  // نوع محتوى الملف مثل "application/pdf"

    [Required]
    public int FileSize { get; set; }  // حجم الملف بالكيلوبايت

    [Required]
    public DateTime UploadDate { get; set; }  // تاريخ رفع الملف

    [StringLength(500)]
    public string FilePath { get; set; }  // مسار تخزين الملف
}
