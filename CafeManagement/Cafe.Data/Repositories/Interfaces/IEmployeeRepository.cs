using Cafe.Data.Entities;

namespace Cafe.Data.Repositories.Interfaces
{
    public interface IEmployeeRepository : IRepository<EmployeeEntity>
    {
        Task<IEnumerable<EmployeeEntity>> GetByCafeIdAsync(Guid cafeId);
        Task<bool> IsEmployeeAssignedToCafe(Guid employeeId);
    }
}
