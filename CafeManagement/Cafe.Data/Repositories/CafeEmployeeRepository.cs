using Cafe.Data.Context;
using Cafe.Data.Entities;
using Cafe.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Data.Repositories
{
    public class CafeEmployeeRepository : Repository<CafeEmployeeEntity>, ICafeEmployeeRepository
    {
        public CafeEmployeeRepository(CafeDbContext context) : base(context)
        {
        }

        public async Task<CafeEmployeeEntity> GetCurrentEmploymentAsync(Guid employeeId)
        {
            return await _context.CafeEmployees
                .FirstOrDefaultAsync(ce => ce.EmployeeId == employeeId && ce.IsActive);
        }

        public async Task<IEnumerable<CafeEmployeeEntity>> GetAllCafeEmployeeByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.CafeEmployees
                .Where(ce => ce.EmployeeId == employeeId).ToListAsync();
        }

        public async Task AssignEmployeeToCafeAsync(Guid employeeId, string employeeName, Guid cafeId, string cafeName)
        {
            // Check current assignment
            var currentAssignment = await GetCurrentEmploymentAsync(employeeId);

            // If already assigned to the same cafe, do nothing
            if (currentAssignment?.CafeId == cafeId)
            {
                return;
            }

            // If has current assignment, deactivate it
            if (currentAssignment != null)
            {
                currentAssignment.IsActive = false;
                currentAssignment.EndDate = DateTime.UtcNow;
            }

            // Create new assignment
            var newAssignment = new CafeEmployeeEntity
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                EmployeeName = employeeName,
                CafeId = cafeId,
                CafeName = cafeName,
                StartDate = DateTime.UtcNow,
                IsActive = true
            };

            await _context.CafeEmployees.AddAsync(newAssignment);
            await _context.SaveChangesAsync();
        }

        public async Task UnassignEmployeeFromCafeAsync(Guid employeeId)
        {
            var currentAssignment = await GetCurrentEmploymentAsync(employeeId);

            if (currentAssignment != null)
            {
                currentAssignment.IsActive = false;
                currentAssignment.EndDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnassignListofEmployeesFromCafeAsync(Guid cafeId)
        {
            var currentAssignments = await GetCurrentEmployeesByCafeAsync(cafeId);

            if (currentAssignments?.Any() == true)
            {
                foreach (var assignment in currentAssignments)
                {
                    assignment.IsActive = false;
                    assignment.EndDate = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<CafeEmployeeEntity>> GetCurrentEmployeesByCafeAsync(Guid cafeId)
        {
            return await _context.CafeEmployees
                .Where(ce => ce.CafeId == cafeId && ce.IsActive)
                .OrderByDescending(ce => ce.StartDate)
                .ToListAsync();
        }
    }
}