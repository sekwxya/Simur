namespace TourAgency.Models
{
    public class CountryStatistics
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public int TourCount { get; set; }
        public decimal Revenue { get; set; }
    }
}
