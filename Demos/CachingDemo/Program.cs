#region SETUP

using CachingDemo.Models;
using CachingDemo.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

Stopwatch stopwatch = Stopwatch.StartNew();
var cacheManager = new CacheManager(
    new MemoryCache(new MemoryCacheOptions()));
var productService = new ProductService(cacheManager);
var priceService = new PriceService(cacheManager);

#endregion

#region FIRST REQUEST - NOT CACHED

// Retrieve product data - won't have been cached.
// Request overhead because of missing cached data.
var product = productService.GetProduct(1);
Console.WriteLine($"{product} at {stopwatch.Elapsed}");

// Retrieve price data - won't have been cached.
// Request overhead because of missing cached data.
var productPrice = priceService.GetProductPrice(product.Id);
Console.WriteLine($"{productPrice} at {stopwatch.Elapsed}");

Console.WriteLine("---------");
stopwatch.Restart();

#endregion

#region SECOND REQUEST - CACHED

// Retrieve product data - has been cached.
// No request overhead because of the cache.
product = productService.GetProduct(1);
Console.WriteLine($"{product} at {stopwatch.Elapsed}");

// Retrieve price data - has been cached.
// No request overhead because of the cache.
productPrice = priceService.GetProductPrice(product.Id);
Console.WriteLine($"{productPrice} at {stopwatch.Elapsed}");

Console.WriteLine("---------");

#endregion

#region THIRD REQUEST - CACHE EXPIRED

priceService.UpdatePrice(new ProductPrice() { Id = 1, ProductId = 1, Price = 2399.99m, DiscountPercentage = 0 });

Console.WriteLine("~~~ WAITING 5 SEC (until cache entries expire)");
Task.Delay(5100).GetAwaiter().GetResult();
stopwatch.Restart();

// Retrieve product data - has been cached.
// No request overhead because of the cache.
product = productService.GetProduct(1);
Console.WriteLine($"{product} at {stopwatch.Elapsed}");

// Retrieve price data - has been cached.
// Request overhead because of missing cached data.
// Price has been updated.
productPrice = priceService.GetProductPrice(product.Id);
Console.WriteLine($"{productPrice} at {stopwatch.Elapsed}");

#endregion