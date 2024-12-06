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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsHot { get; set; } // Флаг "горящего тура"
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<TourPlan> TourPlans { get; set; }
    }
}
