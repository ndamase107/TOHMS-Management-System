using System.ComponentModel.DataAnnotations;

namespace TOHMS.Models
{
    public class Certificate
    {
        public int CertificateId { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string CertificateNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string CertificateType { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [StringLength(100)]
        public string Status { get; set; } = "Active";

        public Employee? Employee { get; set; }
    }
}