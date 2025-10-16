using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models.Invoice.PurchasesInvoice
{
    public class PurchaseInvoiceItem
    {
        public int Id { get; set; }

        [Required]
        public int PurchaseInvoiceId { get; set; }

        [ForeignKey("PurchaseInvoiceId")]
        public virtual PurchaseInvoice? PurchaseInvoice { get; set; }

        [Required]
        public int CategoryId { get; set; }  // Category of item purchased

        [ForeignKey("CategoryId")]
        public virtual Category.Category? Category { get; set; }
        public decimal? Percentage { get; set; } = 0;
        public bool IsPercentageAdded { get; set; } = true;

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public decimal Total
        {
            get
            {
                var baseTotal = Quantity * UnitPrice;
                if (IsPercentageAdded)
                    return baseTotal * (1 + ((Percentage ?? 0) / 100));
                else
                    return baseTotal;
            }
        }
    }
}
