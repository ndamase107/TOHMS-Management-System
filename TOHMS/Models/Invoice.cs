using System.ComponentModel.DataAnnotations;

namespace TOHMS.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        public int ClientId { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; }

        public DateTime DueDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Unpaid";

        public decimal TotalAmount { get; set; }

        public Client Client { get; set; }

        public ICollection<InvoiceItem> InvoiceItems { get; set; } =
            new List<InvoiceItem>();
    }
}