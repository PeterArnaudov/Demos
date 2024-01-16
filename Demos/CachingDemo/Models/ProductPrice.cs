namespace CachingDemo.Models
{
    public class ProductPrice
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal DiscountedPrice => Price * (100 - DiscountPercentage) / 100;

        public bool HasDiscount => DiscountPercentage > 0;

        public override string ToString()
        {
            return $"Price {{{Id}}} " +
                $"{(HasDiscount ? $"{DiscountedPrice.ToString("F2")} ({DiscountPercentage}% OFF)" : $"{Price.ToString("F2")}")}";
        }
    }
}
