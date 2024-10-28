using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cafe.Data.Context
{
    public class CafeDbContextFactory : IDesignTimeDbContextFactory<CafeDbContext>
    {
        public CafeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CafeDbContext>();
            optionsBuilder.UseSqlite("Data Source=Database/cafe.db");
            return new CafeDbContext(optionsBuilder.Options);
        }
    }
}
