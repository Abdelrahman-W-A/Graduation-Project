using System.Diagnostics;
using Demo.BLL;
using Demo.DAL.Data.DbContex;
using Demo.DAL.Models;
using Demo.PL.Models;
using Demo.PL.PdfGenerators;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        #region Index
        public IActionResult Index()
        {
            var totalSales = _context.SalesInvoices
                .Sum(t => (decimal?)t.TotalAmount) ?? 0;

            var totalPurchases = _context.PurchaseInvoices
                .Sum(t => (decimal?)t.TotalAmount) ?? 0;

            var totalSalaries = _context.Employees
                .Sum(e => (decimal?)e.Salary) ?? 0;

            var topClients = _context.SalesInvoices
                .GroupBy(si => si.Client.Name)
                .Select(g => new TopClientDto
                {
                    ClientName = g.Key,
                    TotalPurchased = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(x => x.TotalPurchased)
                .Take(5)
                .ToList();

            var topSellers = _context.PurchaseInvoices
                .GroupBy(pi => pi.Seller.Name)
                .Select(g => new TopSellerDto
                {
                    SellerName = g.Key,
                    TotalSupplied = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(x => x.TotalSupplied)
                .Take(5)
                .ToList();

            var topCategories = _context.SalesInvoiceItems
                .GroupBy(i => i.Category.Name)
                .Select(g => new TopCategoryDto
                    {
                        CategoryName = g.Key,
                        TotalAmount = g.Sum(i => i.Quantity * i.UnitPrice)
                    })
                .OrderByDescending(x => x.TotalAmount)
                .Take(5)
                .ToList();

            var model = new Demo.PL.ViewModels.HomeViewModel
            {
                DepartmentsCount = _context.Departments.Count(),
                EmployeesCount = _context.Employees.Count(),
                UsersCount = _context.Users.Count(),
                TotalSales = totalSales,
                TotalPurchases = totalPurchases,
                EmployeesTotalSalaries = totalSalaries,
                TopClients = topClients,
                TopSellers = topSellers,
                TopCategories = topCategories
            };

            return View(model);
        }
        #endregion

        #region Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region PDF
        public IActionResult GenerateDashboardPdf()
        {
            var totalSales = _context.SalesInvoices.Sum(t => (decimal?)t.TotalAmount) ?? 0;
            var totalPurchases = _context.PurchaseInvoices.Sum(t => (decimal?)t.TotalAmount) ?? 0;
            var totalSalaries = _context.Employees.Sum(e => (decimal?)e.Salary) ?? 0;

            var topClients = _context.SalesInvoices
                .GroupBy(si => si.Client.Name)
                .Select(g => new TopClientDto
                {
                    ClientName = g.Key,
                    TotalPurchased = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(x => x.TotalPurchased)
                .Take(5)
                .ToList();

            var topSellers = _context.PurchaseInvoices
                .GroupBy(pi => pi.Seller.Name)
                .Select(g => new TopSellerDto
                {
                    SellerName = g.Key,
                    TotalSupplied = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(x => x.TotalSupplied)
                .Take(5)
                .ToList();

            var topCategories = _context.SalesInvoiceItems
                .GroupBy(i => i.Category.Name)
                .Select(g => new TopCategoryDto
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(i => i.Quantity * i.UnitPrice)
                })
                .OrderByDescending(x => x.TotalAmount)
                .Take(5)
                .ToList();

            var model = new Demo.PL.ViewModels.HomeViewModel
            {
                DepartmentsCount = _context.Departments.Count(),
                EmployeesCount = _context.Employees.Count(),
                UsersCount = _context.Users.Count(),
                TotalSales = totalSales,
                TotalPurchases = totalPurchases,
                EmployeesTotalSalaries = totalSalaries,
                TopClients = topClients,
                TopSellers = topSellers,
                TopCategories = topCategories
            };

            var pdfBytes = DashboardPdfGenerator.Generate(model);
            return File(pdfBytes, "application/pdf", "DashboardReport.pdf");
        }
        #endregion

    }
}
