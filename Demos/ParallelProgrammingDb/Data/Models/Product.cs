namespace ParallelProgramming.Data.Models
{
    public class Product : BaseModel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public DateTime? ValidUntil { get; set; }
    }
}
