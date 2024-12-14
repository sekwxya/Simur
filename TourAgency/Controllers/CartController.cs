using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourAgency.Data;
using TourAgency.Models;

namespace TourAgency.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity?.Name;
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return Unauthorized();
            }

            var tourPlans = await _context.TourPlan
                .Include(tp => tp.Tour)
                .ThenInclude(t => t.Discount) // Загружаем связанные скидки
                .Where(tp => tp.UserId == user.UserId)
                .ToListAsync();

            return View(tourPlans);
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int tourPlanId)
        {
            var tourPlan = await _context.TourPlan.FindAsync(tourPlanId);
            if (tourPlan != null)
            {
                _context.TourPlan.Remove(tourPlan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MoveToTourHistory(int tourPlanId)
        {
            var tourPlan = await _context.TourPlan.Include(tp => tp.Tour).FirstOrDefaultAsync(tp => tp.TourPlanId == tourPlanId);
            if (tourPlan != null)
            {
                // Добавление тура в TourHistory
                var tourHistory = new TourHistory
                {
                    UserId = tourPlan.UserId,
                    TourId = tourPlan.TourId,
                    Status = "Completed"
                };
                _context.TourHistory.Add(tourHistory);

                // Удаление из турплана
                _context.TourPlan.Remove(tourPlan);

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
