using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourAgency.Data;
using TourAgency.Models;
using System.Security.Claims;

namespace TourAgency.Controllers
{
    [Authorize]
    public class PersonalAccountController : Controller
    {
        private readonly AppDbContext _context;

        public PersonalAccountController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name);

            if (userEmail == null)
            {
                return Unauthorized("Пользователь не авторизован.");
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            return View(user);
        }
    }
}
