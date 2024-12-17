using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TourAgency.Data;
using TourAgency.Models;

namespace TourAgency.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        // Показ всех отзывов пользователя
        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.User
                .Include(u => u.Reviews)
                .ThenInclude(r => r.Tour)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                return Unauthorized("Пользователь не найден.");

            return View(user.Reviews);
        }

        // Добавление отзыва
        public async Task<IActionResult> Add(int tourId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                return Unauthorized("Пользователь не найден.");

            // Проверка, посещал ли пользователь тур
            var tourHistory = await _context.TourHistory
                .FirstOrDefaultAsync(th => th.UserId == user.UserId && th.TourId == tourId);

            if (tourHistory == null)
                return BadRequest("Вы не посещали данный тур.");

            // Проверка, оставлен ли уже отзыв
            var existingReview = await _context.Rewiew
                .FirstOrDefaultAsync(r => r.UserId == user.UserId && r.TourId == tourId);

            if (existingReview != null)
                return BadRequest("Вы уже оставили отзыв на этот тур.");

            return View(new Review { TourId = tourId });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Review review)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                return Unauthorized("Пользователь не найден.");

            review.UserId = user.UserId;

            _context.Rewiew.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Review review)
        {
            _context.Rewiew.Update(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    if (!_context.Tour.Any(t => t.TourId == review.TourId))
                    {
                        ModelState.AddModelError("", "Выбранный тур не существует.");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // Удаление отзыва
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Rewiew.FindAsync(id);

            if (review == null)
                return NotFound("Отзыв не найден.");

            _context.Rewiew.Remove(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> SelectTourForReview()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                return Unauthorized("Пользователь не найден.");

            // Получаем туры, которые пользователь посетил, но ещё не оставил отзыв
            var visitedTours = await _context.TourHistory
                .Include(th => th.Tour)
                .Where(th => th.UserId == user.UserId)
                .Select(th => th.Tour)
                .ToListAsync();

            var reviewedTours = await _context.Rewiew
                .Where(r => r.UserId == user.UserId)
                .Select(r => r.TourId)
                .ToListAsync();

            var toursForReview = visitedTours
                .Where(t => !reviewedTours.Contains(t.TourId))
                .ToList();

            return View(toursForReview);
        }
        //public async Task<IActionResult> GetReviewsByTour(int tourId)
        //{
        //    var reviews = await _context.Rewiew
        //        .Include(r => r.User)
        //        .Where(r => r.TourId == tourId)
        //        .Select(r => new
        //        {
        //            r.Comment,
        //            r.Rating,
        //            UserName = r.User.FullName
        //        })
        //        .ToListAsync();

        //    return PartialView("_TourReviewsPartial", reviews);
        //}

    }
}
