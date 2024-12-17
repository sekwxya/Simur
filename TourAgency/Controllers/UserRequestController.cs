using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TourAgency.Data;
using TourAgency.Models;

namespace TourAgency.Controllers
{
    [Authorize]
    public class UserRequestController : Controller
    {
        private readonly AppDbContext _context;

        public UserRequestController(AppDbContext context)
        {
            _context = context;
        }

        // Отображение заявок текущего пользователя
        public async Task<IActionResult> Index()
        {
            // Получаем Email текущего пользователя
            var userEmail = User.FindFirstValue(ClaimTypes.Name);

            if (userEmail == null)
            {
                return Unauthorized("Пользователь не авторизован.");
            }

            // Получаем текущего пользователя
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            // Получаем заявки пользователя
            var userRequests = await _context.TourRequest
                .Include(tr => tr.Tour) // Включаем информацию о туре
                .Where(tr => tr.UserId == user.UserId)
                .ToListAsync();

            return View(userRequests);
        }

        // Детальный просмотр заявки
        public async Task<IActionResult> Details(int id)
        {
            var tourRequest = await _context.TourRequest
                .Include(tr => tr.Tour)
                .FirstOrDefaultAsync(tr => tr.TourRequestId == id);

            if (tourRequest == null)
            {
                return NotFound("Заявка не найдена.");
            }

            return View(tourRequest);
        }
    }
}
