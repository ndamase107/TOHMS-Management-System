using TOHMS.Models;

namespace TOHMS.Repositories.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<IEnumerable<Client>> GetActiveClientsAsync();

        Task<Client?> GetClientWithEmployeesAsync(int id);
    }
}