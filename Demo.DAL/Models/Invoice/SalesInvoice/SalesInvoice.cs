using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models.Invoice.SalesInvoice
{

    public class SalesInvoice : BaseInvoice
    {
        [Required]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client.Client? Client { get; set; }

        // Many categories can be sold in one invoice
        public virtual List<SalesInvoiceItem> Items { get; set; } = new List<SalesInvoiceItem>();
    }
}
