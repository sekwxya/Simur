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
        public async Task<IActionResult> Index()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return Unauthorized();
            }

            var tours = await _context.Tour
                .Include(t => t.Discount)
                .Include(t => t.reviews)
                .ToListAsync();

            // Горячие туры (по конкретной скидке)
            var hotTours = tours.Where(t => t.Discount != null && t.Discount.DiscountId == 2).ToList();

            // Рекомендации (только с высоким средним рейтингом и ограничение на 5)
            var recommendedTours = tours
                .Where(t => t.AverageRating >= 4 &&
                            !_context.TourHistory.Any(th => th.UserId == user.UserId && th.TourId == t.TourId))
                .OrderByDescending(t => t.AverageRating)
                .Take(5)
                .ToList();

            // Обычные туры (оставшиеся, без учета фильтров для рекомендаций)
            var regularTours = tours.Where(t => t.Discount == null || t.Discount.DiscountId != 2).ToList();

            ViewBag.HotTours = hotTours;
            ViewBag.RecommendedTours = recommendedTours;
            ViewBag.RegularTours = regularTours;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult SubmitTourRequest(string preferences)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = _context.User.FirstOrDefault(x => x.Email == email);

            if (user == null || string.IsNullOrWhiteSpace(preferences))
            {
                return BadRequest("Комментарий не может быть пустым.");
            }

            var tourRequest = new TourRequest
            {
                Status = "На рассмотрении",
                UserId = user.UserId,
                Preferences = preferences,
            };

            _context.Add(tourRequest);
            _context.SaveChanges();

            return Ok(new { message = "Заявка успешно отправлена!" });
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddToTourPlan(int tourId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = _context.User.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                return Unauthorized(new { message = "Пользователь не найден." });
            }

            var existingPlan = _context.TourPlan.FirstOrDefault(tp => tp.UserId == user.UserId && tp.TourId == tourId);
            if (existingPlan != null)
            {
                return BadRequest(new { message = "Тур уже добавлен в ваш Турплан." });
            }

            var tourPlan = new TourPlan
            {
                UserId = user.UserId,
                TourId = tourId
            };

            _context.TourPlan.Add(tourPlan);
            _context.SaveChanges();

            return Ok(new { message = "Тур успешно добавлен в Турплан!" });
        }

        public IActionResult Urna()
        {
            return Redirect("https://ru.wikipedia.org/wiki/%D0%9F%D0%BE%D0%B3%D1%80%D0%B5%D0%B1%D0%B0%D0%BB%D1%8C%D0%BD%D0%B0%D1%8F_%D1%83%D1%80%D0%BD%D0%B0");
        }
    }
}
