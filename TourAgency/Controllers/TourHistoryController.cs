using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TourAgency.Data;
using TourAgency.Models;

namespace TourAgency.Controllers
{
    [Authorize]
    public class TourHistoryController : Controller
    {
        private readonly AppDbContext _context;

        public TourHistoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("Пользователь не авторизован.");
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            var tourHistory = await _context.TourHistory
                .Include(th => th.Tour)
                .Where(th => th.UserId == user.UserId)
                .ToListAsync();

            return View(tourHistory);
        }
    }
}
