using TOHMS.Models;
using TOHMS.Repositories.Interfaces;
using TOHMS.Services.Interfaces;

namespace TOHMS.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Client client)
        {
            await _repository.AddAsync(client);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client client)
        {
            _repository.Update(client);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var client = await _repository.GetByIdAsync(id);

            if (client != null)
            {
                _repository.Delete(client);
                await _repository.SaveChangesAsync();
            }
        }
    }
}