
        namespace Demo.PL.ViewModels
        {
            public class DepartmentDetailsViewModel
            {
                public int Id { get; set; }
                public string Code { get; set; } = string.Empty;
                public string Name { get; set; } = string.Empty;
                public string? Description { get; set; }
                public DateTime CreatedOn { get; set; }

                public int EmployeeCount { get; set; }

                public decimal TotalSalaries { get; set; }

                public List<EmployeeViewModel> Employees { get; set; } = new();
            }
        }

