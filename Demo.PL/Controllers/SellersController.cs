using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.DAL;
using Demo.DAL.Models.Seller;
using Demo.DAL.Data.DbContex;

namespace Demo.PL.Controllers
{
    public class SellersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SellersController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index(string? searchName)
        {
            var sellersQuery = _context.Sellers
                .Include(s => s.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
            {
                sellersQuery = sellersQuery.Where(s => s.Name.Contains(searchName));
            }

            var sellers = await sellersQuery.ToListAsync();
            ViewBag.SearchName = searchName; 
            return View(sellers);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var seller = await _context.Sellers
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (seller == null) return NotFound();

            return View(seller);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(
                _context.Categories
                    .Where(c => c.CategoryType == CategoryTypes.Purchased),
                "Id",
                "Name"
            );
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seller);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(
                _context.Categories
                    .Where(c => c.CategoryType == CategoryTypes.Purchased),
                "Id",
                "Name",
                seller.CategoryId
            );

            return View(seller);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null) return NotFound();

            ViewBag.Categories = new SelectList(
                _context.Categories
                    .Where(c => c.CategoryType == CategoryTypes.Purchased),
                "Id",
                "Name",
                seller.CategoryId
            );

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (id != seller.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seller);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Sellers.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(
                _context.Categories
                    .Where(c => c.CategoryType == CategoryTypes.Purchased),
                "Id",
                "Name",
                seller.CategoryId
            );

            return View(seller);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var seller = await _context.Sellers
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (seller == null) return NotFound();

            return View(seller);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seller = await _context.Sellers.FindAsync(id);

            if (seller == null)
            {
                TempData["DeleteError"] = "Seller not found.";
                return RedirectToAction(nameof(Index));
            }

            bool usedInSales = await _context.SalesInvoices.AnyAsync(s => s.ClientId == id);
            bool usedInPurchases = await _context.PurchaseInvoices.AnyAsync(p => p.SellerId == id);

            if (usedInSales || usedInPurchases)
            {
                TempData["DeleteError"] =
                    "This seller cannot be deleted because it is being used in purchases invoices.";
                return RedirectToAction(nameof(Index));
            }

            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync();

            TempData["DeleteSuccess"] = "Seller deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
