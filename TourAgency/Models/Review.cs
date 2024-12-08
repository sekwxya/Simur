namespace TourAgency.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } // Оценка от 1 до 5
        public User User { get; set; }
        public Tour Tour { get; set; }
    }
}
