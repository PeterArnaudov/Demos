using Microsoft.EntityFrameworkCore;
using ParallelProgramming.Data;
using ParallelProgramming.Data.Models;

namespace ParallelProgramming.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext dbContext;

        public ProductService(
            ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Product AddProduct(
            Product product)
        {
            lock (this.dbContext)
            {
                this.dbContext.Products.Add(product);
                this.dbContext.SaveChanges();
            }

            return product;
        }

        public int AddProducts(
            IEnumerable<Product> products)
        {
            lock (this.dbContext)
            {
                this.dbContext.Products.AddRange(products);
                return this.dbContext.SaveChanges();
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            lock (this.dbContext)
            {
                return this.dbContext.Products.ToArray();
            }
        }
    }
}
