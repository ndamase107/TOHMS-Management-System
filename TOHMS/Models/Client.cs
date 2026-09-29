using System.ComponentModel.DataAnnotations;

namespace TOHMS.Models
{
    public class Client
    {
        public int ClientId { get; set; }

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(100)]
        public string ContactPerson { get; set; } = string.Empty;

        [Phone]
        [StringLength(30)]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [StringLength(300)]
        public string Address { get; set; } = string.Empty;

        [StringLength(100)]
        public string Industry { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Employee> Employees { get; set; } =
            new List<Employee>();

        public ICollection<Risk> Risks { get; set; } =
            new List<Risk>();

        public ICollection<Invoice> Invoices { get; set; } =
            new List<Invoice>();
    }
}