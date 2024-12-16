using Microsoft.AspNetCore.Mvc;
using TourAgency.Data;
using TourAgency.Models;

namespace TourAgency.Controllers
{
    public class PersonalAccountController : Controller
    {
        private readonly AppDbContext _context;

        public PersonalAccountController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var user = new User
            {
                FullName = "Имя Пользователя",
                Balance = 12500.75m,
                LoyaltyPoints = 320
            };
            return View(user);
        }
    }
}
