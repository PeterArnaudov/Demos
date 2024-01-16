#region SETUP

using ConsoleApp1;
using System.Diagnostics;

Stopwatch stopwatch = Stopwatch.StartNew();
var customerService = new CustomerService();
var stockMarketService = new StockMarketService();
var foodService = new FoodService();

var stockMarketName = "Meta";
var sandwichTypes = new[] { "classic", "granny's", "blt" };
var sauces = new[] { "ketchup", "mayonnaise", "mustard" };
var extras = new[] { "cucumbers", "tomatoes", "olives" };

#endregion

#region SYNC

Console.WriteLine("Synchronous exection demo:");
var customer = await customerService.GetCurrentCustomerAsync(stopwatch);
var stockMarket = await stockMarketService.GetStockMarketAsync(
    stockMarketName, stopwatch);

var customerStockValueSync = await stockMarketService.GetCustomerStockValueAsync(
    customer, stockMarket, stopwatch);
Console.WriteLine($"Total time elapsed: {stopwatch.Elapsed}.");

Console.WriteLine("---------");
stopwatch.Restart();

#endregion

#region ASYNC

Console.WriteLine("Asynchronous exection demo:");
var customerTask = customerService.GetCurrentCustomerAsync(stopwatch);
var stockMarketTask = stockMarketService.GetStockMarketAsync(
    stockMarketName, stopwatch);

var customerStockValueAsync = await stockMarketService.GetCustomerStockValueAsync(
    await customerTask, await stockMarketTask, stopwatch);
Console.WriteLine($"Total time elapsed: {stopwatch.Elapsed}.");

Console.WriteLine("---------");
stopwatch.Restart();

#endregion

#region WHEN ALL

Console.WriteLine("When.All() demo:");
var sandwichTasksWhenAll = sandwichTypes.Select(x =>
{
    return foodService.PrepareSandwichAsync(x, stopwatch);
}).ToList();

var sandwiches = await Task.WhenAll(sandwichTasksWhenAll);

Console.WriteLine($"Sandwiches ready to be served at {stopwatch.Elapsed}:\n" +
    $"{string.Join("\n", sandwiches.Select(x => x.ToString()))}");
Console.WriteLine($"Total time elapsed: {stopwatch.Elapsed}.");

Console.WriteLine("---------");
stopwatch.Restart();

#endregion

#region WHEN ANY

Console.WriteLine("When.Any() demo:");
var sandwichTasksWhenAny = sandwichTypes.Select(async x =>
{
    var sandwich = await foodService.PrepareSandwichAsync(x, stopwatch);

    await Task.WhenAll(new[]
    {
        foodService.AddSauceAsync(sandwich, sauces[0], stopwatch),
        foodService.AddExtraAsync(sandwich, extras[0], stopwatch)
    });

    return sandwich;
}).ToList();

while (sandwichTasksWhenAny.Count > 0)
{
    var sandwichTask = await Task.WhenAny(sandwichTasksWhenAny);
    sandwichTasksWhenAny.Remove(sandwichTask);

    Console.WriteLine($"Sandwich ready to be served: {sandwichTask.Result} at {stopwatch.Elapsed}!");
}

Console.WriteLine($"All sandwiches ready to consume at {stopwatch.Elapsed}");
Console.WriteLine($"Total time elapsed: {stopwatch.Elapsed}.");

#endregion