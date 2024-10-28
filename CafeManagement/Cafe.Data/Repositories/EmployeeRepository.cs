using Cafe.Data.Context;
using Cafe.Data.Entities;
using Cafe.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Data.Repositories
{
    public class EmployeeRepository : Repository<EmployeeEntity>, IEmployeeRepository
    {
        public EmployeeRepository(CafeDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EmployeeEntity>> GetByCafeIdAsync(Guid cafeId)
        {
            var activeAssignments = await _context.CafeEmployees
                .Where(ce => ce.CafeId == cafeId && ce.IsActive)
                .OrderByDescending(ce => ce.StartDate)
                .ToListAsync();

            var employeeIds = activeAssignments.Select(a => a.EmployeeId);
            return await _context.Employees
                .Where(e => employeeIds.Contains(e.Id))
                .ToListAsync();
        }

        public async Task<bool> IsEmployeeAssignedToCafe(Guid employeeId)
        {
            return await _context.CafeEmployees
                .AnyAsync(ce => ce.EmployeeId == employeeId && ce.IsActive);
        }

        public async override Task<IEnumerable<EmployeeEntity>>  GetAllAsync()
        {
            return await _context.Employees.Where(ce => ce.IsActive).ToListAsync();
        }
    }
}