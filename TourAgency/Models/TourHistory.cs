namespace TourAgency.Models
{
    public class TourHistory
    {
        public int TourHistoryId { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
        public Tour Tour { get; set; }
    }
}
