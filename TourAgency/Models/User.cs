using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace TourAgency.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int LoyaltyPoints { get; set; }
        public decimal Balance { get; set; }
        public List<Review> Reviews { get; set; } = [];
        public List<TourRequest> TourRequests { get; set; } = [];
    }
}
