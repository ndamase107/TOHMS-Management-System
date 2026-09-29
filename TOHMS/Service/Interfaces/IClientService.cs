using TOHMS.Models;

namespace TOHMS.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAllAsync();

        Task<Client?> GetByIdAsync(int id);

        Task AddAsync(Client client);

        Task UpdateAsync(Client client);

        Task DeleteAsync(int id);
    }
}