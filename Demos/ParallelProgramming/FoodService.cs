using System.Diagnostics;

namespace ConsoleApp1
{
    public class FoodService
    {
        public async Task<Sandwich> PrepareSandwichAsync(
            string sandwichType,
            Stopwatch stopwatch)
        {
            Console.WriteLine($"{nameof(PrepareSandwichAsync)} started executing at {stopwatch.Elapsed}");

            var preparationTime = default(int);
            
            switch (sandwichType)
            {
                case "classic":
                    preparationTime = 1500; break;
                case "granny's":
                    preparationTime = 5000; break;
                case "blt":
                    preparationTime = 2000; break;
                default:
                    break;
            }
            
            await Task.Delay(preparationTime);
            var result = new Sandwich { Type = sandwichType.ToUpper() };
            Console.WriteLine($"Sandwich ready: {result} at {stopwatch.Elapsed}");

            return result;
        }

        public async Task<string> AddSauceAsync(
            Sandwich sandwich,
            string sauce,
            Stopwatch stopwatch)
        {
            Console.WriteLine($"{nameof(AddSauceAsync)} started executing at {stopwatch.Elapsed}");
            await Task.Delay(2000);
            var result = sandwich.Sauce = sauce.ToUpper();
            Console.WriteLine($"Finished adding sauce: {result} at {stopwatch.Elapsed}");

            return result;
        }

        public async Task<string> AddExtraAsync(
            Sandwich sandwich,
            string extra,
            Stopwatch stopwatch)
        {
            Console.WriteLine($"{nameof(AddExtraAsync)} started executing at {stopwatch.Elapsed}");
            await Task.Delay(2000);
            var result = sandwich.Extra = extra.ToUpper();
            Console.WriteLine($"Finished adding extra: {result} at {stopwatch.Elapsed}");

            return result;
        }
    }
}
