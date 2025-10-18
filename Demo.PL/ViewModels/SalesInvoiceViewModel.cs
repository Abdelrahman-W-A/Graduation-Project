using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class SalesInvoiceViewModel
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
        public decimal? Percentage { get; set; }
        public bool IsPercentageAdded { get; set; }


        [Required]

        public int ClientId { get; set; }

        public IFormFile? Attachment { get; set; }

        public List<SalesInvoiceItemViewModel> Items { get; set; } = new();
    }

    public class SalesInvoiceItemViewModel
    {
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Percentage { get; set; } = 0;
        public bool IsPercentageAdded { get; set; } = true;

    }
}
