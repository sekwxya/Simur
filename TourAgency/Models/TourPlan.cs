namespace TourAgency.Models
{
    public class TourPlan
    {
        public int TourPlanId { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public virtual User User { get; set; }
        public virtual Tour Tour { get; set; }
    }
}
