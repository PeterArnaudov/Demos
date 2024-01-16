using CachingDemo.Models;

namespace CachingDemo.Services
{
    public class PriceService
    {
        private const string ProductPriceCacheKeyFormat = "price:productId-{0}";
        private const int ProductPriceCacheDuration = 5;

        private readonly CacheManager cacheManager;
        private readonly List<ProductPrice> prices;

        public PriceService(
            CacheManager cacheManager)
        {
            this.cacheManager = cacheManager;

            // Mock data
            prices = new List<ProductPrice>()
            {
                new ProductPrice() { Id = 1, ProductId = 1, Price = 2399.99m, DiscountPercentage = 10m },
                new ProductPrice() { Id = 2, ProductId = 2, Price = 2000m, DiscountPercentage = 0m  },
                new ProductPrice() { Id = 3, ProductId = 3, Price = 2349.99m, DiscountPercentage = 15m },
                new ProductPrice() { Id = 4, ProductId = 4, Price = 149.99m, DiscountPercentage = 30m },
                new ProductPrice() { Id = 5, ProductId = 5, Price = 499.99m, DiscountPercentage = 0m },
            };
        }

        public ProductPrice GetProductPrice(
            int productId)
        {
            var cacheKey = string.Format(ProductPriceCacheKeyFormat, productId);
            var cachedPrice = cacheManager.Get<ProductPrice>(cacheKey);

            if (cachedPrice != null)
            {
                return cachedPrice;
            }

            // Simulate request overhead.
            Task.Delay(2000).GetAwaiter().GetResult();
            var price = prices.Single(p => p.ProductId == productId);

            cacheManager.Set(cacheKey, price, ProductPriceCacheDuration);

            return price;
        }

        public void UpdatePrice(
            ProductPrice price)
        {
            var index = prices.IndexOf(
                prices.Single(p => p.ProductId == price.ProductId));

            prices[index] = price;

            // Invalidate (remove) cache entry.
            var cacheKey = string.Format(ProductPriceCacheKeyFormat, price.ProductId);
            cacheManager.Remove(cacheKey);
        }
    }
}
