using CachingDemo.Models;

namespace CachingDemo.Services
{
    public class ProductService
    {
        private const string ProductCacheKeyFormat = "product:{0}";
        private const int ProductCacheDuration = 5;

        private readonly CacheManager cacheManager;
        private readonly List<Product> products;

        public ProductService(
            CacheManager cacheManager)
        {
            this.cacheManager = cacheManager;

            // Mock data
            products = new List<Product>()
            {
                new Product() { Id = 1, Name = "iPhone 14 Pro" },
                new Product() { Id = 2, Name = "Sony HT-7000" },
                new Product() { Id = 3, Name = "Samsung Oddysey G9" },
                new Product() { Id = 4, Name = "Logitech G413" },
                new Product() { Id = 5, Name = "Razer Leviathan V2" },
            };
        }

        public Product GetProduct(
            int id)
        {
            var cacheKey = string.Format(ProductCacheKeyFormat, id);
            var cachedProduct = cacheManager.Get<Product>(cacheKey);

            if (cachedProduct != null)
            {
                return cachedProduct;
            }

            // Simulate request overhead.
            Task.Delay(2000).GetAwaiter().GetResult();
            var product = products.Single(p => p.Id == id);

            cacheManager.Set(cacheKey, product, ProductCacheDuration);

            return product;
        }
    }
}
