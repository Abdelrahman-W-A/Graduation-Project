using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.DAL.Models.Invoice.SalesInvoice;
using Demo.DAL.Data.DbContex;
using Demo.DAL;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Net.Mail;
using Demo.PL.ViewModels;

namespace Demo.PL.Controllers
{
    public class SalesInvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesInvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Index
        public IActionResult InvoicesIndex(string searchCode, DateTime? fromDate, DateTime? toDate)
        {
            var salesInvoicesQuery = _context.SalesInvoices
                .Include(i => i.Client)
                .Include(i => i.Items)
                .ThenInclude(it => it.Category)
                .AsQueryable();

            var purchaseInvoicesQuery = _context.PurchaseInvoices
                .Include(p => p.Seller)
                .Include(p => p.Items)
                .ThenInclude(it => it.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchCode))
            {
                salesInvoicesQuery = salesInvoicesQuery.Where(i => i.InvoiceCode.Contains(searchCode));
                purchaseInvoicesQuery = purchaseInvoicesQuery.Where(i => i.InvoiceCode.Contains(searchCode));
            }
            else if (fromDate.HasValue && toDate.HasValue)
            {
                salesInvoicesQuery = salesInvoicesQuery.Where(i => i.InvoiceDate >= fromDate.Value && i.InvoiceDate <= toDate.Value);
                purchaseInvoicesQuery = purchaseInvoicesQuery.Where(i => i.InvoiceDate >= fromDate.Value && i.InvoiceDate <= toDate.Value);
            }

            var viewModel = new Demo.PL.ViewModels.InvoicesViewModel
            {
                SalesInvoices = salesInvoicesQuery.ToList(),
                PurchaseInvoices = purchaseInvoicesQuery.ToList()
            };

            ViewBag.SearchCode = searchCode;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(viewModel);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.SalesInvoices
                .Include(s => s.Client)
                .Include(s => s.Items)
                .ThenInclude(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (invoice == null) return NotFound();

            return View(invoice);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new SalesInvoiceViewModel();

            model.Items = new List<SalesInvoiceItemViewModel>
                    {
                        new SalesInvoiceItemViewModel()
                    };

            ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.CategoryType == CategoryTypes.Sold), "Id", "Name");

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesInvoice model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new SalesInvoiceViewModel
                {
                    InvoiceCode = model.InvoiceCode,
                    InvoiceDate = model.InvoiceDate,
                    ClientId = model.ClientId,
                    Items = model.Items?.Select(i => new SalesInvoiceItemViewModel
                    {
                        CategoryId = i.CategoryId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Percentage = i.Percentage,
                        IsPercentageAdded = i.IsPercentageAdded
                    }).ToList() ?? new List<SalesInvoiceItemViewModel>()
                };

                ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name", model.ClientId);
                ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.CategoryType == CategoryTypes.Sold), "Id", "Name");

                return View(viewModel);
            }


            var invoice = new SalesInvoice
            {
                InvoiceCode = model.InvoiceCode,
                InvoiceDate = model.InvoiceDate,
                ClientId = model.ClientId,
                Items = new List<SalesInvoiceItem>(),
                TotalAmount = 0
            };

            // 🔹 Loop through items and calculate percentage logic
            if (model.Items != null && model.Items.Any())
            {
                foreach (var item in model.Items)
                {
                    var total = item.Quantity * item.UnitPrice;
                    var percentage = item.Percentage ?? 0;
                    var finalTotal = item.IsPercentageAdded
                        ? total + (total * percentage / 100)
                        : total - (total * percentage / 100);

                    invoice.TotalAmount += finalTotal;

                    invoice.Items.Add(new SalesInvoiceItem
                    {
                        CategoryId = item.CategoryId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Percentage = percentage,
                        IsPercentageAdded = item.IsPercentageAdded
                    });

                    var category = await _context.Categories.FindAsync(item.CategoryId);
                    if (category != null)
                        category.AmountNumber -= item.Quantity;
                }
            }

            // 🔹 Handle file attachment
            if (Request.Form.Files.Count > 0)
            {
                using var ms = new MemoryStream();
                await Request.Form.Files[0].CopyToAsync(ms);
                invoice.AttachmentData = ms.ToArray();
                invoice.AttachmentFileName = Request.Form.Files[0].FileName;
                invoice.AttachmentContentType = Request.Form.Files[0].ContentType;
            }

            _context.Add(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction("InvoicesIndex","SalesInvoices");
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int id)
        {
            var invoice = await _context.SalesInvoices
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (invoice == null) return NotFound();

            ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name", invoice.ClientId);
            ViewBag.Categories = new SelectList(
                _context.Categories.Where(c => c.CategoryType == CategoryTypes.Sold),
                "Id", "Name");

            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalesInvoice model)
        {
            if (id != model.Id)
                return NotFound();

            var invoice = await _context.SalesInvoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound();

            invoice.InvoiceCode = model.InvoiceCode;
            invoice.InvoiceDate = model.InvoiceDate;
            invoice.ClientId = model.ClientId;
            invoice.TotalAmount = 0;

            // Remove old items
            _context.SalesInvoiceItems.RemoveRange(invoice.Items);
            invoice.Items.Clear();

            // Add updated items with recalculation
            foreach (var item in model.Items)
            {
                var total = item.Quantity * item.UnitPrice;
                var percentage = item.Percentage ?? 0;
                var finalTotal = item.IsPercentageAdded
                    ? total + (total * percentage / 100)
                    : total - (total * percentage / 100);

                invoice.TotalAmount += finalTotal;

                invoice.Items.Add(new SalesInvoiceItem
                {
                    CategoryId = item.CategoryId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Percentage = percentage,
                    IsPercentageAdded = item.IsPercentageAdded
                });

                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == item.CategoryId);
                if (category != null)
                    category.AmountNumber -= item.Quantity;
            }

            // Update attachment if new file uploaded
            if (Request.Form.Files.Count > 0)
            {
                using var ms = new MemoryStream();
                await Request.Form.Files[0].CopyToAsync(ms);
                invoice.AttachmentData = ms.ToArray();
                invoice.AttachmentFileName = Request.Form.Files[0].FileName;
                invoice.AttachmentContentType = Request.Form.Files[0].ContentType;
            }

            _context.Update(invoice);
            await _context.SaveChangesAsync();

            return RedirectToAction("InvoicesIndex","SalesInvoices");
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.SalesInvoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (invoice == null) return NotFound();

            return View(invoice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.SalesInvoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null) return NotFound();

            foreach (var item in invoice.Items)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == item.CategoryId);
                if (category != null)
                {
                    category.AmountNumber += item.Quantity;
                }
            }

            _context.SalesInvoiceItems.RemoveRange(invoice.Items);
            _context.SalesInvoices.Remove(invoice);

            await _context.SaveChangesAsync();

            return RedirectToAction("InvoicesIndex","SalesInvoices");
        }
        #endregion

        #region PDF
        public static byte[] GenerateSalesInvoicePdf(SalesInvoice invoice)
        {
            var generatedDateTime = DateTime.Now;

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.Size(PageSizes.A4);

                    // 🔹 Header
                    page.Header()
                        .Column(column =>
                        {
                            column.Item().Text("Sales Invoice").FontSize(24).Bold();
                            column.Item().Text($"Invoice Code: {invoice.InvoiceCode}").FontSize(12);
                            column.Item().Text($"Client: {invoice.Client?.Name ?? ""}").FontSize(12);
                            column.Item().Text($"Invoice Date: {invoice.InvoiceDate:yyyy-MM-dd}").FontSize(12);
                            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        });

                    // 🔹 Content Table
                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Item
                                columns.RelativeColumn(2); // Quantity
                                columns.RelativeColumn(2); // Unit Price
                                columns.RelativeColumn(2); // Percentage
                                columns.RelativeColumn(2); // Type
                                columns.RelativeColumn(2); // Total
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Text("Item").SemiBold();
                                header.Cell().Text("Quantity").SemiBold().AlignCenter();
                                header.Cell().Text("Unit Price").SemiBold().AlignCenter();
                                header.Cell().Text("Percentage (%)").SemiBold().AlignCenter();
                                header.Cell().Text("Type").SemiBold().AlignCenter();
                                header.Cell().Text("Total").SemiBold().AlignCenter();
                            });

                            decimal grandTotal = 0;

                            foreach (var item in invoice.Items)
                            {
                                // Safely handle nulls
                                decimal quantity = item.Quantity;
                                decimal unitPrice = item.UnitPrice;
                                decimal percentage = item.Percentage ?? 0;

                                // Base total
                                decimal baseTotal = quantity * unitPrice;

                                // Apply percentage
                                decimal percentValue = baseTotal * (percentage / 100);
                                decimal finalTotal = item.IsPercentageAdded ? baseTotal + percentValue : baseTotal - percentValue;

                                grandTotal += finalTotal;

                                // Add cells
                                table.Cell().Text(item.Category?.Name ?? "");
                                table.Cell().Text(quantity.ToString("0.##")).AlignCenter();
                                table.Cell().Text(unitPrice.ToString("0.00")).AlignCenter();
                                table.Cell().Text($"{percentage.ToString("0.##")} %").AlignCenter();
                                table.Cell().Text(item.IsPercentageAdded ? "Added (+)" : "Subtracted (−)").AlignCenter();
                                table.Cell().Text(finalTotal.ToString("0.00")).AlignCenter();
                            }

                            // Footer (Total)
                            table.Footer(footer =>
                            {
                                footer.Cell().ColumnSpan(5).AlignRight().Text("Total Cost").Bold();
                                footer.Cell().Text(grandTotal.ToString("0.00")).Bold().AlignCenter();
                            });
                        });

                    // 🔹 Footer
                    page.Footer()
                        .AlignCenter()
                        .Column(column =>
                        {
                            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                            column.Item().Text($"Generated by SCS on {generatedDateTime:yyyy-MM-dd HH:mm:ss}").FontSize(10);
                        });
                });
            });

            return doc.GeneratePdf();
        }





        public IActionResult GeneratePdf(int id)
        {
            var invoice = _context.SalesInvoices
                .Include(i => i.Client)
                .Include(i => i.Items)
                .ThenInclude(it => it.Category)
                .FirstOrDefault(i => i.Id == id);

            if (invoice == null) return NotFound();

            var pdfBytes = GenerateSalesInvoicePdf(invoice);
            return File(pdfBytes, "application/pdf", $"SalesInvoice_{invoice.Id}.pdf");
        }

        public async Task<IActionResult> DownloadAttachment(int id)
        {
            var invoice = await _context.SalesInvoices.FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null || invoice.AttachmentData == null) return NotFound();

            return File(invoice.AttachmentData, invoice.AttachmentContentType ?? "application/octet-stream", invoice.AttachmentFileName);
        }

        public async Task<IActionResult> ViewAttachment(int id)
        {
            var invoice = await _context.SalesInvoices.FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null || invoice.AttachmentData == null)
                return NotFound();

            return File(invoice.AttachmentData, invoice.AttachmentContentType ?? "application/octet-stream");
        }
        #endregion

    }

}


