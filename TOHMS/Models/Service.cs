using System.ComponentModel.DataAnnotations;

namespace TOHMS.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        [Required]
        [StringLength(150)]
        public string ServiceName { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 100000000)]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<InvoiceItem> InvoiceItems { get; set; } =
            new List<InvoiceItem>();
    }
}