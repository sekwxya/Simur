using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace TourAgency.Models
{
    public class User
    {пше 
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Visitor" или "TourManager"
        public int LoyaltyPoints { get; set; } // Бонусные баллы
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<TourRequest> TourRequests { get; set; }
    }
}
