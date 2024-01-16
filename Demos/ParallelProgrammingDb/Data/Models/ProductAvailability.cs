namespace ParallelProgramming.Data.Models
{
    public class ProductAvailability : BaseModel
    {
        public int ProductId { get; set; }

        public int AvailableUnits { get; set; }
    }
}
