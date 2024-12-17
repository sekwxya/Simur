namespace TourAgency.Models
{
    public class TourRequest
    {
        public int TourRequestId { get; set; }
        public int UserId { get; set; }
        public string Preferences { get; set; }
        public string? Status { get; set; }
        public int? TourId { get; set; }
        public Tour? Tour { get; set; }
        public User User { get; set; }
    }
}
