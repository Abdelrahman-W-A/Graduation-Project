namespace Demo.PL.ViewModels
{
    public class HomeViewModel
    {
        public int DepartmentsCount { get; set; }
        public int EmployeesCount { get; set; }
        public int UsersCount { get; set; }

        public decimal TotalSales { get; set; }
        public decimal TotalPurchases { get; set; }
        public decimal NetProfit => TotalSales - TotalPurchases;
        public decimal EmployeesTotalSalaries { get; set; }

        public List<TopClientDto> TopClients { get; set; } = new();
        public List<TopSellerDto> TopSellers { get; set; } = new();
        public List<TopCategoryDto> TopCategories { get; set; } = new();

    }

    public class TopClientDto
    {
        public string ClientName { get; set; }
        public decimal TotalPurchased { get; set; }
    }

    public class TopSellerDto
    {
        public string SellerName { get; set; }
        public decimal TotalSupplied { get; set; }
    }

    public class TopCategoryDto
    {
        public string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
