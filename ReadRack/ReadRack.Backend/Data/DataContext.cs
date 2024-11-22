using Microsoft.EntityFrameworkCore;
using ReadRack.Shared.Entites;

namespace ReadRack.Backend.Data
{
    public class DataContext:DbContext
    {

        public DataContext(DbContextOptions<DataContext> options):base(options) 
        {
                
        }

        public DbSet<College> colleges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<College>().HasIndex(c=>c.Name).IsUnique();
        }
    }
}
