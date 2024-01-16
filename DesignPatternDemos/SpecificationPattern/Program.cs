global using SpecificationPattern.Data;
global using SpecificationPattern.Data.Models;
global using SpecificationPattern.Interfaces;
global using SpecificationPattern.Services;
global using SpecificationPattern.Specifications;
global using SpecificationPattern.Helpers;
global using System.Linq.Expressions;

Console.WriteLine("Hello, World!");

// Declare services
var dbContext = new ApplicationDbContext();
var productsService = new ProductsService(dbContext);

// Setup
SeedData(dbContext);

// Execute
var specifications = new List<ISpecification<Product>>();
//specifications.Add(new ProductNameSpecification(new string[] { "Coca-Cola" }));
//specifications.Add(new ProductLowerPriceSpecification(3m));
//specifications.Add(new ProductHigherPriceSpecification(2m));
//specifications.Add(new AndSpecification<Product>(new ProductLowerPriceSpecification(3m), new ProductHigherPriceSpecification(2m)));
//specifications.Add(new OrSpecification<Product>(new ProductLowerPriceSpecification(3m), new ProductHigherPriceSpecification(2m)));
//specifications.Add(new OrSpecification<Product>(
//    new ProductCodeSpecification("295-784-586"),
//    new ProductCodeSpecification("295-784-588"),
//    new ProductCodeSpecification("295-784-590")));
//specifications.Add(new ProductExpressionSpecification(p => new int[] { 1, 2, 3, 4 }.Contains(p.Id)));

var sortSpecifications = new List<ISortSpecification<Product>>();
sortSpecifications.Add(new DecimalSortSpecification<Product>(x => x.Price));
sortSpecifications.Add(new DecimalSortSpecification<Product>(x => x.Quantity));

var products = productsService.GetProducts(specifications, sortSpecifications);

Console.WriteLine(string.Join("\n", products.Select(p => $"{p.Name} {p.Quantity}{p.Unit} for ${p.Price}")));

#region Setup methods

void SeedData(ApplicationDbContext dbContext)
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

    dbContext.Products.AddRange(products);
    dbContext.SaveChanges();
}

#endregion
