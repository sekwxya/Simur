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

        // Метод для проверки, является ли тур "горящим"
        public bool IsHotTour()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var daysUntilStart = StartDate.DayNumber - today.DayNumber;
            return daysUntilStart <= 7; // Тур считается "горящим", если до начала осталось 7 дней или меньше
        }

        // Метод для расчета цены с учетом скидки
        public decimal GetDiscountedPrice(Discount hotTourDiscount)
        {
            var basePrice = Price;
            if (Discount != null)
            {
                basePrice -= basePrice * Discount.DiscountPercentage;
            }
            if (IsHotTour() && DiscountId == 1) // Применяем скидку на "горящие туры" только если DiscountId = 1
            {
                basePrice -= basePrice * hotTourDiscount.DiscountPercentage;
            }
            return basePrice;
        }
    }

}
