using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models.AttendanceModel;
using Demo.DAL.Models.Category;
using Demo.DAL.Models.Client;
using Demo.DAL.Models.DepartmentModel;
using Demo.DAL.Models.EmployeeModel;
using Demo.DAL.Models.IDentityModel;
using Demo.DAL.Models.Invoice.PurchasesInvoice;
using Demo.DAL.Models.Invoice.SalesInvoice;
using Demo.DAL.Models.Seller;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Data.DbContex
{
    public class ApplicationDbContext : IdentityDbContext<Application_User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        //public DbSet<SaleTransaction> SaleTransactions { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

            // PURCHASE INVOICE ITEMS
            modelBuilder.Entity<PurchaseInvoiceItem>()
                .HasOne(pi => pi.PurchaseInvoice)
                .WithMany(p => p.Items)
                .HasForeignKey(pi => pi.PurchaseInvoiceId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade delete

            modelBuilder.Entity<PurchaseInvoiceItem>()
                .HasOne(pi => pi.Category)
                .WithMany()
                .HasForeignKey(pi => pi.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // SALES INVOICE ITEMS
            modelBuilder.Entity<SalesInvoiceItem>()
                .HasOne(si => si.SalesInvoice)
                .WithMany(s => s.Items)
                .HasForeignKey(si => si.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade delete

            modelBuilder.Entity<SalesInvoiceItem>()
                .HasOne(si => si.Category)
                .WithMany()
                .HasForeignKey(si => si.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .Property(c => c.AmountNumber)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseInvoice>()
                .Property(p => p.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseInvoiceItem>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesInvoice>()
                .Property(s => s.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesInvoiceItem>()
                .Property(si => si.UnitPrice)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
