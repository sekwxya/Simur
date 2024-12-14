namespace TourAgency.Models
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public string Name { get; set; }
        public decimal DiscountPercentage { get; set; }
        public List<Tour> tours { get; set; } = new();
    }
}
