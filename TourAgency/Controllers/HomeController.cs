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
            var tours = await _context.Tour
                .Include(t => t.Discount) // Включаем связанные скидки
                .ToListAsync();

            var hotTours = tours.Where(t => t.Discount != null && t.Discount.DiscountId == 2).ToList(); //kal
            var regularTours = tours.Where(t => t.Discount == null || t.Discount.DiscountId != 2).ToList();

            ViewBag.HotTours = hotTours;
            ViewBag.RegularTours = regularTours;

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

        [Authorize]
        [HttpPost]
        public IActionResult AddToTourPlan(int tourId)
        {
            var email = User.FindFirst(ClaimTypes.Name).Value;
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
