namespace TourAgency.Models
{
    public class TourPlan
    {
        public int TourPlanId { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public User User { get; set; }
        public Tour Tour { get; set; }
    }
}
