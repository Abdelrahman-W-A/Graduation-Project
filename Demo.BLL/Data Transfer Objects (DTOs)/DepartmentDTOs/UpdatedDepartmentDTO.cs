using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Data_Transfer_Objects__DTOs_
{
    public class UpdatedDepartmentDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateOnly CreatedDate { get; set; }
        public string code { get; set; } = string.Empty;

    }
}
