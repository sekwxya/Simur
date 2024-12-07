namespace TourAgency.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } // Оценка от 1 до 5
        public virtual User User { get; set; }
        public virtual Tour Tour { get; set; }
    }
}
