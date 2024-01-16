using SpecificationPattern.Enums;
using SpecificationPattern.Specifications;
using System.Diagnostics;

namespace SpecificationPattern.Tests
{
    [TestFixture]
    public class SpecificationTests
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
        public void ProductNameSpecificationShouldReturnCorrectResults()
        {
            var productName = "Coca-Cola";

            var specifications = new List<ISpecification<Product>>
            {
                new StringSpecification<Product>(productName, x => x.Name)
            };

            var products = this.productsService.GetProducts(specifications);

            Assert.That(products, Has.Count.EqualTo(products.Where(p => p.Name == productName).Count()));
            Assert.That(products.All(p => p.Name == productName));
        }

        [Test]
        public void ProductNameSpecificationWithListParameterShouldReturnCorrectResults()
        {
            var productNames = new string[] { "Coca-Cola", "Fries" };

            var specifications = new List<ISpecification<Product>>
            {
                new StringSpecification<Product>(productNames, x => x.Name,
                    logicalOperator: LogicalOperator.Or),
            };

            var products = this.productsService.GetProducts(specifications);

            Assert.That(products, Has.Count.EqualTo(products.Where(p => productNames.Contains(p.Name)).Count()));
            Assert.That(products.All(p => productNames.Contains(p.Name)));
        }

        [Test]
        public void ProductNameContainsSpecificationShouldReturnCorrectResults()
        {
            var productName = "Strawberry";

            var specifications = new List<ISpecification<Product>>
            {
                new StringSpecification<Product>(
                    productName, x => x.Name, StringFilterCondition.Contains)
            };

            var products = this.productsService.GetProducts(specifications);

            Assert.That(products, Has.Count.EqualTo(products.Where(p => p.Name.Contains(productName)).Count()));
            Assert.That(products.All(p => p.Name.Contains(productName)));
        }

        [Test]
        public void ProductLowerPriceSpecificationShouldReturnCorrectResults()
        {
            var price = 5m;

            var specifications = new List<ISpecification<Product>>
            {
                new DecimalSpecification<Product>(
                    price, x => x.Price, NumberFilterCondition.IsLessThan)
            };

            var products = this.productsService.GetProducts(specifications);

            Assert.That(products, Has.Count.EqualTo(products.Where(p => p.Price <= price).Count()));
            Assert.That(products.All(p => p.Price <= price));
        }

        [Test]
        public void ProductHigherPriceSpecificationShouldReturnCorrectResults()
        {
            var price = 5m;

            var specifications = new List<ISpecification<Product>>
            {
                new DecimalSpecification<Product>(
                    price, x => x.Price, NumberFilterCondition.IsHigherThan)
            };

            var products = this.productsService.GetProducts(specifications);

            Assert.That(products, Has.Count.EqualTo(products.Where(p => p.Price >= price).Count()));
            Assert.That(products.All(p => p.Price >= price));
        }

        [Test]
        public void ProductMultipleSpecificationsShouldReturnCorrectResults()
        {
            var productName = "Strawberry";
            var price = 5m;

            var specifications = new List<ISpecification<Product>>
            {
                new StringSpecification<Product>(
                    productName, x => x.Name, StringFilterCondition.Contains),
                new DecimalSpecification<Product>(
                    price, x => x.Price, NumberFilterCondition.IsHigherThan)
            };

            var products = this.productsService.GetProducts(specifications);

            Assert.That(products, Has.Count.EqualTo(products.Where(p => p.Name.Contains(productName) && p.Price >= price).Count()));
            Assert.That(products.All(p => p.Name.Contains(productName) && p.Price >= price));
        }

        [Test]
        public void AndSpecificationsShouldReturnCorrectResults()
        {
            var upperLimit = 4m;
            var lowerLimit = 8m;

            var specifications = new List<ISpecification<Product>>
            {
                new AndSpecification<Product>(
                    new DecimalSpecification<Product>(
                        lowerLimit, x => x.Price, NumberFilterCondition.IsHigherThan),
                    new DecimalSpecification<Product>(
                        upperLimit, x => x.Price, NumberFilterCondition.IsLessThan))
            };

            var products = this.productsService.GetProducts(specifications);

            Assert.That(products, Has.Count.EqualTo(products.Where(p => p.Price >= lowerLimit && p.Price <= upperLimit).Count()));
            Assert.That(products.All(p => p.Price >= lowerLimit && p.Price <= upperLimit));
        }

        [Test]
        public void OrSpecificationsShouldReturnCorrectResults()
        {
            var strawberryLemonadeName = "Homemade Strawberry Lemonade";
            var chocolateCakeName = "Chocolate Cake";

            var specifications = new List<ISpecification<Product>>
            {
                new OrSpecification<Product>(
                    new StringSpecification<Product>(strawberryLemonadeName, x => x.Name),
                    new StringSpecification<Product>(chocolateCakeName, x => x.Name))
            };

            var products = this.productsService.GetProducts(specifications);

            Assert.That(products, Has.Count.EqualTo(products.Where(p => p.Name == strawberryLemonadeName || p.Name == chocolateCakeName).Count()));
            Assert.Multiple(() =>
            {
                Assert.That(products.Any(p => p.Name.Contains(strawberryLemonadeName)));
                Assert.That(!products.Any(p => p.Name.Contains(chocolateCakeName)));
            });
        }

        [Test]
        public void OrSpecificationsWithMultipleCriteriaShouldReturnCorrectResults()
        {
            var codes = new string[] { "295-784-586", "295-784-588", "295-784-590" };

            var specifications = new List<ISpecification<Product>>
            {
                new OrSpecification<Product>(
                    new StringSpecification<Product>(codes[0], x => x.Code),
                    new StringSpecification<Product>(codes[1], x => x.Code),
                    new StringSpecification<Product>(codes[2], x => x.Code))
            };

            var products = this.productsService.GetProducts(specifications);

            Assert.That(products, Has.Count.EqualTo(codes.Length));
            Assert.Multiple(() =>
            {
                Assert.That(products.Any(p => p.Code.Equals(codes[0])));
                Assert.That(products.Any(p => p.Code.Equals(codes[1])));
                Assert.That(products.Any(p => p.Code.Equals(codes[2])));
            });
        }

        private void SeedData(ApplicationDbContext dbContext)
        {
            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();
        }
    }
}