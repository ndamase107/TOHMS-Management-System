using Microsoft.EntityFrameworkCore;
using TOHMS.Data;
using TOHMS.Models;
using TOHMS.Repositories.Interfaces;

namespace TOHMS.Repositories
{
    public class ClientRepository :
        Repository<Client>,
        IClientRepository
    {
        public ClientRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Client>> GetActiveClientsAsync()
        {
            return await _context.Clients
                .Where(c => c.IsActive)
                .OrderBy(c => c.CompanyName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Client?> GetClientWithEmployeesAsync(int id)
        {
            return await _context.Clients
                .Include(c => c.Employees)
                .FirstOrDefaultAsync(c => c.ClientId == id);
        }
    }
}