using Cafe.Data.Entities;

namespace Cafe.Data.Repositories.Interfaces
{
    public interface ICafeEmployeeRepository : IRepository<CafeEmployeeEntity>
    {
        Task<CafeEmployeeEntity> GetCurrentEmploymentAsync(Guid employeeId);
        Task AssignEmployeeToCafeAsync(Guid employeeId, string EmployeeName, Guid cafeId , string CafeName);
        Task UnassignEmployeeFromCafeAsync(Guid employeeId);
        Task<IEnumerable<CafeEmployeeEntity>> GetCurrentEmployeesByCafeAsync(Guid cafeId);
        Task<IEnumerable<CafeEmployeeEntity>> GetAllCafeEmployeeByEmployeeIdAsync(Guid employeeId);
        Task UnassignListofEmployeesFromCafeAsync(Guid cafeId);
    }
}
