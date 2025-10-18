using Demo.BLL.Services.Model_Services.ClientService;
using Demo.DAL.Data.DbContex;
using Demo.DAL.Models.Client;
using Microsoft.EntityFrameworkCore;

namespace Demo.BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _context.Clients
                                         .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client> CreateAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client?> UpdateAsync(Client client)
        {
            var existing = await _context.Clients.FindAsync(client.Id);
            if (existing == null) return null;

            // Update fields you allow to change
            existing.Name = client.Name;
            existing.PhoneNumber = client.PhoneNumber;
            existing.Address = client.Address;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
