using Microsoft.EntityFrameworkCore;
using ParallelProgramming.Data.Models;

namespace ParallelProgramming.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductAvailability> Availability { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseInMemoryDatabase("ParallelProgrammingDb");
        }
    }
}
