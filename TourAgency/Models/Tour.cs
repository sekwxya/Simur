using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace TourAgency.Models
{
    public class Tour
    {
        public int TourId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public decimal Price { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int DiscountId { get; set; }
        public Discount? Discount { get; set; }
        public List<TourPlan> tourPlans { get; set; } = new();
        public List<Review> reviews { get; set; } = new();
    }
}
