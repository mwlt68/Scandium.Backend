using Microsoft.EntityFrameworkCore;
using Scandium.Model;

namespace Scandium.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.ApplyConfiguration(new UserConfigurations());
        }

        public DbSet<User> Users => Set<User>();
    }
}