using System.ComponentModel.DataAnnotations;

namespace TOHMS.Models
{
    public class Examination
    {
        public int ExaminationId { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string ExaminationType { get; set; } = string.Empty;

        public DateTime ExaminationDate { get; set; }

        [StringLength(50)]
        public string FitnessStatus { get; set; } = "Pending";

        [StringLength(1000)]
        public string Findings { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Recommendations { get; set; } = string.Empty;

        public Employee? Employee { get; set; }
    }
}