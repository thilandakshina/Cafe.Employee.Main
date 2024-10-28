using Cafe.Data.Entities;

namespace Cafe.Data.Repositories.Interfaces
{
    public interface ICafeRepository : IRepository<CafeEntity>
    {
        Task<IEnumerable<CafeEntity>> GetByLocationAsync(string location);
        Task<int> GetEmployeeCountAsync(Guid cafeId);
        Task<CafeEntity> GetCafeByIdAsync(Guid? cafeId);
    }
}
