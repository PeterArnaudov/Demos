namespace SpecificationPattern.Tests
{
    [TestFixture]
    public class SortSpecificationTests
    {
        private readonly Product[] products = new Product[]
        {
            new Product() { Id = 1, Code = "295-784-586", Name = "Coca-Cola", Unit = "ml", Quantity = 330, Price = 3.50m, ValidUntil = new DateTime(2025, 12, 30) },
            new Product() { Id = 2, Code = "295-784-587", Name = "Coca-Cola", Unit = "ml", Quantity = 250, Price = 2.99m, ValidUntil = new DateTime(2025, 12, 30) },
            new Product() { Id = 3, Code = "295-784-588", Name = "Homemade Strawberry Lemonade", Unit = "ml", Quantity = 500, Price = 4.99m, ValidUntil = new DateTime(2025, 12, 30) },
            new Product() { Id = 4, Code = "295-784-589", Name = "Homemade Strawberry Lemonade", Unit = "ml", Quantity = 1000, Price = 8.99m, ValidUntil = new DateTime(2025, 12, 30) },
            new Product() { Id = 5, Code = "295-784-590", Name = "Fries", Unit = "g", Quantity = 250, Price = 4.80m, ValidUntil = new DateTime(2025, 12, 30) },
            new Product() { Id = 6, Code = "295-784-591", Name = "Homemade Strawberry Lemonade PROMO", Unit = "ml", Quantity = 1250, Price = 8.99m, ValidUntil = new DateTime(2023, 5, 30) },
            new Product() { Id = 7, Code = "295-784-592", Name = "Homemade Strawberry Lemonade PROMO OLD", Unit = "ml", Quantity = 1250, Price = 8.99m, ValidUntil = new DateTime(2023, 5, 27) },
        };

        private ApplicationDbContext dbContext;
        private ProductsService productsService;

        [SetUp]
        public void Setup()
        {
            dbContext = new ApplicationDbContext();
            productsService = new ProductsService(dbContext);

            if (!dbContext.Products.Any())
            {
                SeedData(dbContext);
            }
        }

        [Test]
        public void SortSpecificationDescendingShouldReturnSortedResults()
        {
            var sortSpecifications = new List<ISortSpecification<Product>>()
            {
                new DecimalSortSpecification<Product>(x => x.Price),
            };

            var products = this.productsService.GetProducts(
                sortSpecifications: sortSpecifications);

            for (int i = 0; i < products.Count - 2; i++)
            {
                Assert.That(products[i].Price, Is.GreaterThanOrEqualTo(products[i + 1].Price));
            }
        }

        [Test]
        public void SortSpecificationAscendingShouldReturnSortedResults()
        {
            var sortSpecifications = new List<ISortSpecification<Product>>()
            {
                new DecimalSortSpecification<Product>(x => x.Price, true),
            };

            var products = this.productsService.GetProducts(
                sortSpecifications: sortSpecifications);

            for (int i = 0; i < products.Count - 2; i++)
            {
                Assert.That(products[i].Price, Is.LessThanOrEqualTo(products[i + 1].Price));
            }
        }

        [Test]
        public void MultipleSortSpecificationsShouldReturnSortedResults()
        {
            var sortSpecifications = new List<ISortSpecification<Product>>()
            {
                new DecimalSortSpecification<Product>(x => x.Price),
                new DecimalSortSpecification<Product>(x => x.Quantity),
            };

            var products = this.productsService.GetProducts(
                sortSpecifications: sortSpecifications);

            for (int i = 0; i < products.Count - 2; i++)
            {
                Assert.That(products[i].Price, Is.GreaterThanOrEqualTo(products[i + 1].Price));

                if (products[i].Price.Equals(products[i + 1].Price))
                {
                    Assert.That(products[i].Quantity, Is.GreaterThanOrEqualTo(products[i + 1].Quantity));
                }
            }
        }

        private void SeedData(ApplicationDbContext dbContext)
        {
            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();
        }
    }
}