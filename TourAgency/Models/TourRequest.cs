namespace TourAgency.Models
{
    public class TourRequest
    {
        public int TourRequestId { get; set; }
        public int UserId { get; set; }
        public string Preferences { get; set; } // Пожелания пользователя
        public string Status { get; set; } // "Pending", "In Progress", "Completed"
        public User User { get; set; }
    }
}
