using System.Diagnostics;

namespace ConsoleApp1
{
    public class CustomerService
    {
        public async Task<string> GetCurrentCustomerAsync(
            Stopwatch stopwatch)
        {
            Console.WriteLine($"{nameof(GetCurrentCustomerAsync)} started executing at {stopwatch.Elapsed}");
            await Task.Delay(2000);
            Console.WriteLine($"{nameof(GetCurrentCustomerAsync)} finished executing at {stopwatch.Elapsed}");

            return "Test Testov";
        }
    }
}
