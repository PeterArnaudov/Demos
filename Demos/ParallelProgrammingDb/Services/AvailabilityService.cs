using ParallelProgramming.Data;
using ParallelProgramming.Data.Models;
using ParallelProgramming.Enums;

namespace ParallelProgramming.Services
{
    public class AvailabilityService
    {
        private readonly ApplicationDbContext dbContext;

        public AvailabilityService(
            ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ProductAvailability AddAvailability(
            ProductAvailability availability)
        {
            lock (this.dbContext)
            {
                dbContext.Availability.Add(availability);
                dbContext.SaveChanges();
            }

            return availability;
        }

        public int AddAvailability(
            IEnumerable<ProductAvailability> availability)
        {
            lock (this.dbContext)
            {
                dbContext.Availability.AddRange(availability);
                return dbContext.SaveChanges();
            }
        }

        public Availability GetProductAvailability(
            int productId)
        {
            Console.WriteLine($"Started execution of {nameof(GetProductAvailability)} for Product ID {productId}.");

            lock (this.dbContext)
            {
                var availability = dbContext.Availability
                    .SingleOrDefault(x => x.ProductId == productId);

                // Simulate logic overhead.
                Thread.Sleep(500);

                if (availability == null)
                {
                    return Availability.OutOfStock;
                }

                if (availability.AvailableUnits > 100)
                {
                    return Availability.InStock;
                }
                else if (availability.AvailableUnits < 100
                    && availability.AvailableUnits > 0)
                {
                    return Availability.LowStock;
                }
            }

            return Availability.OutOfStock;
        }
    }
}
