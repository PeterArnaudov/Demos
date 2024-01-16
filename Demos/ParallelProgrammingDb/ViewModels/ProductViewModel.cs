using ParallelProgramming.Enums;

namespace ParallelProgramming.ViewModels
{
    public class ProductViewModel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public DateTime? ValidUntil { get; set; }

        public Availability Availability { get; set; }

        public string? PackshotUrl { get; set; }
    }
}
