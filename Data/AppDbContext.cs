using Microsoft.EntityFrameworkCore;
using Api.Services;

namespace Api.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ITenantProvider _tenant;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ITenantProvider tenant) : base(options)
        {
            _tenant = tenant;
        }

        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<Tenant> Tenants => Set<Tenant>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasQueryFilter(p => p.TenantId == _tenant.TenantId);

            modelBuilder.Entity<MenuItem>()
                .HasQueryFilter(m => m.TenantId == _tenant.TenantId);
        }
    }
}
