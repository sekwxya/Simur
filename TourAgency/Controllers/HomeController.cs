using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitTourRequest(string preferences)
        {
            if (string.IsNullOrWhiteSpace(preferences))
            {
                return BadRequest("Комментарий не может быть пустым.");
            }

            var tourRequest = new TourRequest
            {
                Status = "На рассмотрении",
                UserId = 1, // Установить ID пользователя вручную (например, для текущего юзера)
                Preferences = preferences,
            };

            _context.Add(tourRequest);
            _context.SaveChanges();

            return Ok(new { message = "Заявка успешно отправлена!" });
        }
    }
}
