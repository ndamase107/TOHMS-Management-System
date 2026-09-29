using System.ComponentModel.DataAnnotations;

namespace TOHMS.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string AppointmentType { get; set; } = string.Empty;

        public DateTime AppointmentDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Scheduled";

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;

        public Employee? Employee { get; set; }
    }
}