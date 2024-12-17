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
                .ThenInclude(t => t.Discount)
                .Where(tp => tp.UserId == user.UserId)
                .ToListAsync();

            ViewData["User"] = user;
            return View(tourPlans);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPurchase(int tourPlanId, bool useLoyaltyPoints)
        {
            var userEmail = User.Identity?.Name;
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return Unauthorized();
            }

            var tourPlan = await _context.TourPlan
                .Include(tp => tp.Tour).ThenInclude(t => t.Discount)
                .FirstOrDefaultAsync(tp => tp.TourPlanId == tourPlanId);

            if (tourPlan == null)
            {
                return NotFound();
            }

            decimal finalPrice = tourPlan.Tour.DiscountedPrice;
            decimal paidWithBalance = finalPrice;
         //   Console.WriteLine();
            if (useLoyaltyPoints && user.LoyaltyPoints > 0)
            {
                // Рассчитываем скидку бонусами, не уменьшая стоимость ниже 1
                decimal loyaltyDiscount = Math.Min((decimal)user.LoyaltyPoints, finalPrice - 1);
                finalPrice -= loyaltyDiscount;
                paidWithBalance = finalPrice;
                user.LoyaltyPoints -= (int)loyaltyDiscount;
            }

            if (user.Balance >= paidWithBalance)
            {
                // Списание с баланса
                user.Balance -= paidWithBalance;

                // Начисление бонусов за ту часть, что оплачена деньгами
                int bonusesEarned = (int)(paidWithBalance / 100) * 5;
                user.LoyaltyPoints += bonusesEarned;

                // Успешная покупка
                var tourHistory = new TourHistory
                {
                    UserId = user.UserId,
                    TourId = tourPlan.TourId,
                    Status = "Completed"
                };

                _context.TourHistory.Add(tourHistory);
                _context.TourPlan.Remove(tourPlan);
            }
            else
            {
                // Недостаточно средств — оплата не удалась
                var tourHistory = new TourHistory
                {
                    UserId = user.UserId,
                    TourId = tourPlan.TourId,
                    Status = "Payment Failed"
                };

                _context.TourHistory.Add(tourHistory);
                _context.TourPlan.Remove(tourPlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
    }
}
