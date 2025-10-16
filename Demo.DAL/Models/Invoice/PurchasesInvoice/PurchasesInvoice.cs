using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models.Invoice.PurchasesInvoice
{
    public class PurchaseInvoice : BaseInvoice
    {
        [Required]
        public int SellerId { get; set; }

        [ForeignKey("SellerId")]
        public virtual Seller.Seller? Seller { get; set; }

        // Many categories can be purchased in one invoice
        public virtual ICollection<PurchaseInvoiceItem> Items { get; set; } = new List<PurchaseInvoiceItem>();
    }
}
