namespace ParallelProgramming.Services
{
    public class PackshotService
    {
        public async Task<string> GetProductPackshotAsync(
            int productId)
        {
            Console.WriteLine($"Started execution of {nameof(GetProductPackshotAsync)} for Product ID {productId}.");

            // Simulate time-consuming API call
            Thread.Sleep(2000);

            return await Task.FromResult($"www.somePackshotUrl.com/products/{productId}");
        }
    }
}
