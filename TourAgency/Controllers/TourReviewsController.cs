using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourAgency.Data;
using TourAgency.Models;

namespace TourAgency.Controllers
{
    public class TourReviewsController : Controller
    {
        private readonly AppDbContext _context;

        public TourReviewsController(AppDbContext context)
        {
            _context = context;
        }

        // Просмотр отзывов для конкретного тура
        public async Task<IActionResult> Index(int tourId)
        {
            var reviews = await _context.Rewiew
                .Where(r => r.TourId == tourId)
                .Include(r => r.Tour)
                .ToListAsync();

            ViewData["TourName"] = (await _context.Tour.FindAsync(tourId))?.Title ?? "Неизвестный тур";
            ViewData["TourId"] = tourId;

            return View(reviews);
        }

        // Удаление отзыва
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Rewiew.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            var tourId = review.TourId;
            _context.Rewiew.Remove(review);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { tourId });
        }
    }
}
