using Microsoft.AspNetCore.Mvc;
using TourAgency.Data;
using TourAgency.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TourAgency.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BalancesController : Controller
    {
        private readonly AppDbContext _context;

        public BalancesController(AppDbContext context) 
        {
            _context = context;
        }

        // GET: Users/AddBalance/5
        public async Task<IActionResult> Add(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/AddBalance/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int id, decimal amount)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("Amount", "Сумма должна быть положительным числом.");
                var user = await _context.User.FindAsync(id);
                return View(user);
            }

            var userToUpdate = await _context.User.FindAsync(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }

            userToUpdate.Balance += amount;
            _context.Update(userToUpdate);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Баланс успешно пополнен!";
            return RedirectToAction(nameof(Index)); // Перенаправить на список пользователей или другую страницу
        }

        // Пример списка пользователей
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }
    }
}
