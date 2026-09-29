using TOHMS.Models;

namespace TOHMS.Repositories.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetEmployeeByEmail(string email);
    }
}