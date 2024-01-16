using System.Diagnostics;

namespace ConsoleApp1
{
    public class StockMarketService
    {
        public async Task<string> GetStockMarketAsync(
            string market,
            Stopwatch stopwatch)
        {
            Console.WriteLine($"{nameof(GetStockMarketAsync)} started executing at {stopwatch.Elapsed}");
            await Task.Delay(2000);
            Console.WriteLine($"{nameof(GetStockMarketAsync)} finished executing at {stopwatch.Elapsed}");

            return market;
        }
        
        public async Task<decimal> GetCustomerStockValueAsync(
            string customer,
            string market,
            Stopwatch stopwatch)
        {
            Console.WriteLine($"{nameof(GetCustomerStockValueAsync)} started executing at {stopwatch.Elapsed}");
            await Task.Delay(2000);
            Console.WriteLine($"{nameof(GetCustomerStockValueAsync)} finished executing at {stopwatch.Elapsed}");

            return 194.655887m;
        }
    }
}
