using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TourAgency.Data;
using TourAgency.Models;


namespace TourAgency.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult SubmitTourRequest(string preferences)
        {
            var email = User.FindFirst(ClaimTypes.Name).Value;
            var user = _context.User.FirstOrDefault(x => x.Email == email);
            if (string.IsNullOrWhiteSpace(preferences))
            {
                return BadRequest("Комментарий не может быть пустым.");
            }

            var tourRequest = new TourRequest
            {
                Status = "На рассмотрении",
                UserId = user.UserId, // Установить ID пользователя вручную (например, для текущего юзера)
                Preferences = preferences,
            };

            _context.Add(tourRequest);
            _context.SaveChanges();

            return Ok(new { message = "Заявка успешно отправлена!" });
        }

        public IActionResult Urna() 
        { 
            return Redirect("https://ru.wikipedia.org/wiki/%D0%9F%D0%BE%D0%B3%D1%80%D0%B5%D0%B1%D0%B0%D0%BB%D1%8C%D0%BD%D0%B0%D1%8F_%D1%83%D1%80%D0%BD%D0%B0");
        }
    }
}
