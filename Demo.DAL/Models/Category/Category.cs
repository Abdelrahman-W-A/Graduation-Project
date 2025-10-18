using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models.Category
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CategoryTypes CategoryType { get; set; }
        public decimal AmountNumber { get; set; }

        public virtual ICollection<Client.Client>? Clients { get; set; }
        public virtual ICollection<Seller.Seller>? Sellers { get; set; }
    }
}
