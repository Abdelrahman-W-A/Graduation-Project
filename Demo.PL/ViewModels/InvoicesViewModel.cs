namespace Demo.PL.ViewModels
{
    public class InvoicesViewModel
    {
        public IEnumerable<Demo.DAL.Models.Invoice.SalesInvoice.SalesInvoice> SalesInvoices { get; set; }
        public IEnumerable<Demo.DAL.Models.Invoice.PurchasesInvoice.PurchaseInvoice> PurchaseInvoices { get; set; }
    }
}
