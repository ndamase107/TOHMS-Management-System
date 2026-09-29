using System.ComponentModel.DataAnnotations;
using TOHMS.Models;

namespace TOHMS.Models
{
    public class Risk
    {
        public int RiskId { get; set; }

        public int ClientId { get; set; }

        [Required]
        [StringLength(200)]
        public string Hazard { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(50)]
        public string RiskLevel { get; set; } = "Medium";

        [StringLength(1000)]
        public string ControlMeasures { get; set; } = string.Empty;

        public DateTime AssessmentDate { get; set; }

        public bool IsActive { get; set; } = true;

        public Client? Client { get; set; }
    }
}