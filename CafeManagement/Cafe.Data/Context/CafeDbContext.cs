using Cafe.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Data.Context
{
    public class CafeDbContext : DbContext
    {
        public CafeDbContext(DbContextOptions<CafeDbContext> options) : base(options)
        {
        }

        public DbSet<CafeEntity> Cafes { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<CafeEmployeeEntity> CafeEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CafeEmployeeEntity>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Foreign keys
                entity.HasOne<CafeEntity>()
                      .WithMany()
                      .HasForeignKey(ce => ce.CafeId);

                entity.HasOne<EmployeeEntity>()
                      .WithMany()
                      .HasForeignKey(ce => ce.EmployeeId);

                // Ensure only one active assignment per employee
                entity.HasIndex(ce => new { ce.EmployeeId, ce.IsActive })
                      .HasFilter("[IsActive] = 1")
                      .IsUnique();
            });
        }
    }
}