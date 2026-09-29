using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace TOHMS.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public int ClientId { get; set; }

        [Required]
        [StringLength(100)]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(20)]
        public string IDNumber { get; set; } = string.Empty;

        [StringLength(150)]
        public string JobTitle { get; set; } = string.Empty;

        [StringLength(150)]
        public string Department { get; set; } = string.Empty;

        [Phone]
        [StringLength(30)]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public DateTime EmploymentDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Client Client { get; set; }

        public ICollection<Appointment> Appointments { get; set; } =
            new List<Appointment>();

        public ICollection<Examination> Examinations { get; set; } =
            new List<Examination>();

        public ICollection<HealthRecord> HealthRecords { get; set; } =
            new List<HealthRecord>();

        public ICollection<Certificate> Certificates { get; set; } =
            new List<Certificate>();
    }
}