using Cafe.Data.Context;
using Cafe.Data.Entities;
using Cafe.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Data.Repositories
{
    public class CafeRepository : Repository<CafeEntity>, ICafeRepository
    {
        public CafeRepository(CafeDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CafeEntity>> GetByLocationAsync(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return await GetAllAsync();

            var cafes = await _context.Cafes
                .Where(c => c.Location.ToLower().Contains(location.ToLower()))
                .ToListAsync();

            var cafeList = new List<(CafeEntity cafe, int employeeCount)>();
            foreach (var cafe in cafes)
            {
                var employeeCount = await GetEmployeeCountAsync(cafe.Id);
                cafeList.Add((cafe, employeeCount));
            }

            return cafeList
                .OrderByDescending(x => x.employeeCount)
                .Select(x => x.cafe);
        }

        public async Task<CafeEntity> GetCafeByIdAsync(Guid? cafeId)
        {
            return await _context.Cafes.FirstOrDefaultAsync(c => c.Id == cafeId);
        }

        public async Task<int> GetEmployeeCountAsync(Guid cafeId)
        {
            return await _context.CafeEmployees
                .CountAsync(ce => ce.CafeId == cafeId && ce.IsActive);
        }

        public async override Task<IEnumerable<CafeEntity>> GetAllAsync()
        {
            return await _context.Cafes.Where(ce => ce.IsActive).ToListAsync();
        }
    }
}