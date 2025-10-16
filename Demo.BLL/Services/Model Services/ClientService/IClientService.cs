using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models.Client;

namespace Demo.BLL.Services.Model_Services.ClientService
{
        public interface IClientService
        {
            Task<IEnumerable<Client>> GetAllAsync();
            Task<Client?> GetByIdAsync(int id);
            Task<Client> CreateAsync(Client client);
            Task<Client?> UpdateAsync(Client client);
            Task<bool> DeleteAsync(int id);
        }

}
