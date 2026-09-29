using Microsoft.EntityFrameworkCore;
using TOHMS.Data;
using TOHMS.Models;
using TOHMS.Repositories.Interfaces;

namespace TOHMS.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<Employee?> GetEmployeeByEmail(string email)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}