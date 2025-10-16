using Demo.BLL.Services;
using Demo.BLL.Services.Model_Services.ClientService;
using Demo.DAL;
using Demo.DAL.Data.DbContex;
using Demo.DAL.Models.Category;
using Demo.DAL.Models.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ApplicationDbContext _context;

        public ClientsController(IClientService clientService, ApplicationDbContext context)
        {
            _clientService = clientService;
            _context = context;
        }

        #region Index
        public async Task<IActionResult> Index(string? searchName)
        {
            var clientsQuery = _context.Clients.Include(c => c.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
            {
                clientsQuery = clientsQuery.Where(c => c.Name.Contains(searchName));
            }

            var clients = await clientsQuery.ToListAsync();
            ViewBag.SearchName = searchName;
            return View(clients);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(
                _context.Categories
                    .Where(c => c.CategoryType == CategoryTypes.Sold),
                "Id",
                "Name"
            );
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", client.CategoryId);
                return View(client);
            }

            await _clientService.CreateAsync(client);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            ViewBag.Categories = new SelectList(
                            _context.Categories
                                .Where(c => c.CategoryType == CategoryTypes.Sold),
                            "Id",
                            "Name"
                        );
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Update(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", client.CategoryId);

            return View(client);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var client = await _context.Clients
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (client == null)
                return NotFound();

            return View(client);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null) return NotFound();

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                TempData["DeleteError"] = "Client not found.";
                return RedirectToAction(nameof(Index));
            }

            bool usedInSales = await _context.SalesInvoices.AnyAsync(s => s.ClientId == id);
            bool usedInPurchases = await _context.PurchaseInvoices.AnyAsync(p => p.SellerId == id);

            if (usedInSales || usedInPurchases)
            {
                TempData["DeleteError"] =
                    "This client cannot be deleted because it is being used in sales invoices.";
                return RedirectToAction(nameof(Index));
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            TempData["DeleteSuccess"] = "Client deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
