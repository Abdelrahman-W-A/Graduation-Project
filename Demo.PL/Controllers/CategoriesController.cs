using Demo.DAL.Data.DbContex;
using Demo.DAL.Models.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index(string? searchName)
        {
            var categoriesQuery = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
            {
                categoriesQuery = categoriesQuery.Where(c => c.Name.Contains(searchName));
            }

            var categories = await categoriesQuery.ToListAsync();
            ViewBag.SearchName = searchName;
            return View(categories);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null) return NotFound();

            return View(category);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Categories.Any(e => e.Id == category.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                TempData["DeleteError"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }

            // Check if category is used anywhere in the system
            bool usedInSales = await _context.SalesInvoiceItems.AnyAsync(i => i.CategoryId == id);
            bool usedInPurchases = await _context.PurchaseInvoiceItems.AnyAsync(i => i.CategoryId == id);
            bool usedInClients = await _context.Clients.AnyAsync(c => c.CategoryId == id);  // 🔹 Client
            bool usedInSellers = await _context.Sellers.AnyAsync(s => s.CategoryId == id);  // 🔹 Seller

            if (usedInSales || usedInPurchases || usedInClients || usedInSellers)
            {
                TempData["DeleteError"] =
                    "This category cannot be deleted because it is being used in invoices, clients or sellers.";
                return RedirectToAction(nameof(Index));
            }

            // Safe to delete
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            TempData["DeleteSuccess"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
