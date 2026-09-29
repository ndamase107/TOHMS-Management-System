using System.ComponentModel.DataAnnotations;

namespace TOHMS.Models
{
    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }

        public int InvoiceId { get; set; }

        public int ServiceId { get; set; }

        [Range(1, 100000)]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public Invoice? Invoice { get; set; }

        public Service? Service { get; set; }
    }
}