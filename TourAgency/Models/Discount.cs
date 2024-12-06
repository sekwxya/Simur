namespace TourAgency.Models
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public int TourId { get; set; }
        public decimal DiscountPercentage { get; set; }
        public virtual Tour Tour { get; set; }
    }
}
