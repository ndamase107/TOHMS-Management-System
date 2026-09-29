using System.ComponentModel.DataAnnotations;

namespace TOHMS.Models
{
    public class HealthRecord
    {
        public int HealthRecordId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime RecordDate { get; set; }

        [StringLength(100)]
        public string RecordType { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Notes { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Recommendations { get; set; } = string.Empty;

        public Employee? Employee { get; set; }
    }
}