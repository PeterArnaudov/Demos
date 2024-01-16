using Microsoft.EntityFrameworkCore;
using SpecificationPattern.Extensions;

namespace SpecificationPattern.Services
{
    public class ProductsService
    {
        private readonly ApplicationDbContext dbContext;

        public ProductsService(
            ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IReadOnlyList<Product> GetProducts(
            IEnumerable<ISpecification<Product>>? specifications = null,
            IEnumerable<ISortSpecification<Product>>? sortSpecifications = null)
        {
            var queryable = this.dbContext.Products.AsQueryable();

            if (specifications != null)
            {
                queryable = queryable.Specify(specifications.ToArray());
            }

            if (sortSpecifications != null)
            {
                queryable = queryable.Sort(sortSpecifications);
            }

            return queryable.ToList();
        }
    }
}
