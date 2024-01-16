#region Usings

using ParallelProgramming.Data;
using ParallelProgramming.Data.Models;
using ParallelProgramming.Services;
using ParallelProgramming.ViewModels;
using System.Diagnostics;

#endregion

#region Setup

var dbContext = new ApplicationDbContext();
var productService = new ProductService(dbContext);
var availabilityService = new AvailabilityService(dbContext);
var packshotService = new PackshotService();

SeedData();

void SeedData()
{
    var products = new Product[]
    {
        new Product() { Id = 1, Code = "295-784-586", Name = "Coca-Cola", Unit = "ml", Quantity = 330, Price = 3.50m, ValidUntil = new DateTime(2025, 12, 30) },
        new Product() { Id = 2, Code = "295-784-587", Name = "Coca-Cola", Unit = "ml", Quantity = 250, Price = 2.99m, ValidUntil = new DateTime(2025, 12, 30) },
        new Product() { Id = 3, Code = "295-784-588", Name = "Homemade Strawberry Lemonade", Unit = "ml", Quantity = 500, Price = 4.99m, ValidUntil = new DateTime(2025, 12, 30) },
        new Product() { Id = 4, Code = "295-784-589", Name = "Homemade Strawberry Lemonade", Unit = "ml", Quantity = 1000, Price = 8.99m, ValidUntil = new DateTime(2025, 12, 30) },
        new Product() { Id = 5, Code = "295-784-590", Name = "Fries", Unit = "g", Quantity = 250, Price = 4.80m, ValidUntil = new DateTime(2025, 12, 30) },
        new Product() { Id = 6, Code = "295-784-591", Name = "Homemade Strawberry Lemonade PROMO", Unit = "ml", Quantity = 1250, Price = 8.99m, ValidUntil = new DateTime(2023, 5, 30) },
        new Product() { Id = 7, Code = "295-784-592", Name = "Homemade Strawberry Lemonade PROMO OLD", Unit = "ml", Quantity = 1250, Price = 8.99m, ValidUntil = new DateTime(2023, 5, 27) },
    };

    var availabilty = new ProductAvailability[]
    {
        new ProductAvailability() { Id = 1, ProductId = 1, AvailableUnits = 350 },
        new ProductAvailability() { Id = 2, ProductId = 2, AvailableUnits = 35 },
        new ProductAvailability() { Id = 3, ProductId = 3, AvailableUnits = 10 },
        new ProductAvailability() { Id = 4, ProductId = 4, AvailableUnits = 0 },
        new ProductAvailability() { Id = 5, ProductId = 5, AvailableUnits = 101 },
        new ProductAvailability() { Id = 6, ProductId = 6, AvailableUnits = 15 },
        new ProductAvailability() { Id = 7, ProductId = 7, AvailableUnits = 50 },
    };

    productService.AddProducts(products);
    availabilityService.AddAvailability(availabilty);
}

#endregion

Stopwatch stopwatch = Stopwatch.StartNew();

var products = productService.GetProducts();

var vmTasks = products.Select(
    async (x) =>
    {
        var viewModel = new ProductViewModel();

        // Run each task asynchronously using Task.Run
        await Task.Run(async () =>
        {
            Console.WriteLine($"Started building a View Model for Product ID {x.Id} at {stopwatch.Elapsed}.");

            // Start each method execution on a new thread
            var packshotTask = Task.Run(() => packshotService.GetProductPackshotAsync(x.Id));
            var availabilityTask = Task.Run(() => availabilityService.GetProductAvailability(x.Id));

            viewModel.Code = x.Code;
            viewModel.Name = x.Name;
            viewModel.Unit = x.Unit;
            viewModel.Quantity = x.Quantity;
            viewModel.Price = x.Price;
            viewModel.ValidUntil = x.ValidUntil;

            // Make sure all methods are executed
            await Task.WhenAll(packshotTask, availabilityTask);

            // Set execution results to the view model
            viewModel.PackshotUrl = await packshotTask;
            viewModel.Availability = await availabilityTask;
        });

        return viewModel;
    });

var viewModels = await Task.WhenAll(vmTasks);

Console.WriteLine($"Finished building view models at {stopwatch.Elapsed}.");
Console.WriteLine(string.Join("\n", viewModels.Select(p => $"{p.Name} {p.Quantity}{p.Unit} for ${p.Price}: {p.Availability}\nImage: {p.PackshotUrl}")));