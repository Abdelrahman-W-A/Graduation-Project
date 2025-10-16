using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.DAL.Models.Invoice.PurchasesInvoice;
using Demo.DAL.Data.DbContex;
using Demo.DAL;
using System.Net.Mail;

namespace Demo.PL.Controllers
{
    public class PurchaseInvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseInvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            var invoices = await _context.PurchaseInvoices
                .Include(p => p.Seller)
                .Include(p => p.Items)
                .ThenInclude(i => i.Category)
                .ToListAsync();
            return View(invoices);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.PurchaseInvoices
                .Include(p => p.Seller)
                .Include(p => p.Items)
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
            var model = new PurchaseInvoice
            {
                Items = new List<PurchaseInvoiceItem>
        {
            new PurchaseInvoiceItem()
        }
            };

            ViewBag.Sellers = new SelectList(_context.Sellers, "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Categories
                .Where(c => c.CategoryType == CategoryTypes.Purchased), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseInvoice model, IFormFile? attachment)
        {
            model.Items ??= new List<PurchaseInvoiceItem>();

            if (!ModelState.IsValid)
            {
                ViewBag.Sellers = new SelectList(_context.Sellers, "Id", "Name", model.SellerId);
                ViewBag.Categories = new SelectList(
                    _context.Categories.Where(c => c.CategoryType == CategoryTypes.Purchased),
                    "Id", "Name"
                );
                return View(model);
            }

            decimal total = 0;
            foreach (var item in model.Items)
            {
                decimal itemTotal = item.Quantity * item.UnitPrice;

                if (item.Percentage.HasValue && item.Percentage.Value != 0)
                {
                    var percentValue = itemTotal * (item.Percentage.Value / 100);
                    itemTotal = item.IsPercentageAdded ? itemTotal + percentValue : itemTotal - percentValue;
                }

                total += itemTotal;
            }

            model.TotalAmount = total;

            if (attachment != null && attachment.Length > 0)
            {
                using var ms = new MemoryStream();
                await attachment.CopyToAsync(ms);
                model.AttachmentData = ms.ToArray();
                model.AttachmentFileName = attachment.FileName;
                model.AttachmentContentType = attachment.ContentType;
            }

            foreach (var item in model.Items)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == item.CategoryId);
                if (category != null)
                    category.AmountNumber += item.Quantity;
            }

            _context.PurchaseInvoices.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("InvoicesIndex", "SalesInvoices");
        }

        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.PurchaseInvoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (invoice == null) return NotFound();

            ViewBag.Sellers = new SelectList(_context.Sellers, "Id", "Name", invoice.SellerId);
            ViewBag.Categories = new SelectList(
                _context.Categories.Where(C => C.CategoryType == CategoryTypes.Purchased),
                "Id", "Name"
            );

            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseInvoice model, IFormFile? attachment)
        {
            if (id != model.Id) return NotFound();

            model.Items ??= new List<PurchaseInvoiceItem>();

            var invoice = await _context.PurchaseInvoices
                .Include(p => p.Items)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (invoice == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Sellers = new SelectList(_context.Sellers, "Id", "Name", model.SellerId);
                ViewBag.Categories = new SelectList(
                    _context.Categories.Where(C => C.CategoryType == CategoryTypes.Purchased),
                    "Id", "Name"
                );
                return View(model);
            }

            // Revert old quantities
            foreach (var oldItem in invoice.Items)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == oldItem.CategoryId);
                if (category != null)
                    category.AmountNumber -= oldItem.Quantity;
            }

            // ✅ Update main fields
            invoice.InvoiceCode = model.InvoiceCode;
            invoice.InvoiceDate = model.InvoiceDate;
            invoice.SellerId = model.SellerId;
            invoice.Percentage = model.Percentage;
            invoice.IsPercentageAdded = model.IsPercentageAdded;

            // ✅ Recalculate total amount with percentage logic
            decimal total = 0;
            foreach (var item in model.Items)
            {
                decimal itemTotal = item.Quantity * item.UnitPrice;
                if (item.Percentage.HasValue && item.Percentage.Value != 0)
                {
                    var percentValue = itemTotal * (item.Percentage.Value / 100);
                    itemTotal = item.IsPercentageAdded ? itemTotal + percentValue : itemTotal - percentValue;
                }
                total += itemTotal;
            }
            invoice.TotalAmount = total;

            // Handle attachment update
            if (attachment != null && attachment.Length > 0)
            {
                using var ms = new MemoryStream();
                await attachment.CopyToAsync(ms);
                invoice.AttachmentData = ms.ToArray();
                invoice.AttachmentFileName = attachment.FileName;
                invoice.AttachmentContentType = attachment.ContentType;
            }

            // Replace items
            _context.PurchaseInvoiceItems.RemoveRange(invoice.Items);
            invoice.Items.Clear();

            foreach (var item in model.Items)
            {
                invoice.Items.Add(new PurchaseInvoiceItem
                {
                    CategoryId = item.CategoryId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Percentage = item.Percentage,
                    IsPercentageAdded = item.IsPercentageAdded
                });

                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == item.CategoryId);
                if (category != null)
                    category.AmountNumber += item.Quantity;
            }

            await _context.SaveChangesAsync();

            // ✅ Redirect correctly
            return RedirectToAction("InvoicesIndex", "SalesInvoices");
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.PurchaseInvoices
                .Include(p => p.Seller)
                .Include(p => p.Items)
                .ThenInclude(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (invoice == null) return NotFound();

            return View(invoice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.PurchaseInvoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice != null)
            {
                foreach (var item in invoice.Items)
                {
                    var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == item.CategoryId);
                    if (category != null)
                    {
                        category.AmountNumber -= item.Quantity;
                    }
                }

                _context.PurchaseInvoiceItems.RemoveRange(invoice.Items);

                _context.PurchaseInvoices.Remove(invoice);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("InvoicesIndex", "SalesInvoices");
        }
        #endregion

        #region PDF
        public IActionResult GeneratePdf(int id)
        {
            var invoice = _context.PurchaseInvoices
                .Include(p => p.Seller)
                .Include(p => p.Items)
                .ThenInclude(i => i.Category)
                .FirstOrDefault(p => p.Id == id);

            if (invoice == null) return NotFound();

            var pdfBytes = PurchaseInvoicePdfGenerator.Generate(invoice);

            return File(pdfBytes, "application/pdf", $"PurchaseInvoice-{invoice.InvoiceCode}.pdf");
        }

        public async Task<IActionResult> DownloadAttachment(int id)
        {
            var invoice = await _context.PurchaseInvoices.FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null || invoice.AttachmentData == null) return NotFound();

            return File(invoice.AttachmentData, invoice.AttachmentContentType ?? "application/octet-stream", invoice.AttachmentFileName);
        }

        public async Task<IActionResult> ViewAttachment(int id)
        {
            var invoice = await _context.PurchaseInvoices.FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null || invoice.AttachmentData == null) return NotFound();

            return File(invoice.AttachmentData, invoice.AttachmentContentType ?? "application/octet-stream");
        }
        #endregion

    }
}
