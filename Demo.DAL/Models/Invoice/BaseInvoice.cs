using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models.Invoice
{
    public class BaseInvoice
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Required]
        public decimal TotalAmount { get; set; }
        public string? InvoiceCode { get; set; }

        // Attachment options
        public string? AttachmentPath { get; set; } // optional, file path
        public byte[]? AttachmentData { get; set; } // optional, file bytes
        public string? AttachmentFileName { get; set; } // optional, original file name
        public string? AttachmentContentType { get; set; }

        [Range(0, 100)]
        public decimal? Percentage { get; set; } // the value of discount or tax

        public bool IsPercentageAdded { get; set; } // true = add, false = subtract


    }

}
