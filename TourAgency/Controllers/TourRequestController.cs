using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourAgency.Data;
using TourAgency.Models;

namespace TourAgency.Controllers
{
    public class TourRequestController : Controller
    {
        private readonly AppDbContext _context;

        public TourRequestController(AppDbContext context)
        {
            _context = context;
        }

        // Список всех заявок
        public async Task<IActionResult> Index()
        {
            var tourRequests = await _context.TourRequest
                .Include(tr => tr.User)
                .Where(tr => tr.Status == "На рассмотрении") // Фильтруем только заявки "На рассмотрении"
                .ToListAsync();

            return View(tourRequests);
        }

        // Отображение деталей заявки
        public async Task<IActionResult> Details(int id)
        {
            var tourRequest = await _context.TourRequest
                .Include(tr => tr.User)
                .FirstOrDefaultAsync(tr => tr.TourRequestId == id);

            if (tourRequest == null)
            {
                return NotFound();
            }

            var tours = await _context.Tour.ToListAsync();
            ViewBag.Tours = tours;

            return View(tourRequest);
        }

        // Подтверждение заявки
        [HttpPost]
        public async Task<IActionResult> Approve(int id, int TourId)
        {
            var tourRequest = await _context.TourRequest.FindAsync(id);

            if (tourRequest == null)
            {
                return NotFound();
            }

            var tour = await _context.Tour.FindAsync(TourId);
            if (tour == null)
            {
                return BadRequest("Выбранный тур не найден.");
            }

            // Обновление статуса заявки
            tourRequest.Status = "Одобрено";

            // Сохранение изменений
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Отклонение заявки
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var tourRequest = await _context.TourRequest.FindAsync(id);

            if (tourRequest == null)
            {
                return NotFound();
            }

            // Обновление статуса заявки
            tourRequest.Status = "Отклонено";

            // Сохранение изменений
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
