﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TourAgency.Data;
using TourAgency.Models;

namespace TourAgency.Controllers
{
    [Authorize]
    public class TourRequestController : Controller
    {
        private readonly AppDbContext _context;

        public TourRequestController(AppDbContext context)
        {
            _context = context;
        }

        // Список заявок со статусом "На рассмотрении"
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var tourRequests = await _context.TourRequest
                .Include(tr => tr.User)
                .Where(tr => tr.Status == "На рассмотрении")
                .ToListAsync();

            return View(tourRequests);
        }

        // Детальный просмотр заявки
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var tourRequest = await _context.TourRequest
                .Include(tr => tr.User)
                .FirstOrDefaultAsync(tr => tr.TourRequestId == id);

            if (tourRequest == null)
            {
                return NotFound();
            }

            // Передаем список туров в ViewBag для выбора
            var tours = await _context.Tour.ToListAsync();
            ViewBag.Tours = tours;

            return View(tourRequest);
        }

        // Подтверждение заявки с сохранением выбранного тура
        [HttpPost]
        [Authorize(Roles = "Admin")]
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

            // Обновляем заявку: присваиваем TourId и статус "Одобрено"
            tourRequest.TourId = TourId;
            tourRequest.Status = "Одобрено";

            _context.Update(tourRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Отклонение заявки
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var tourRequest = await _context.TourRequest.FindAsync(id);

            if (tourRequest == null)
            {
                return NotFound();
            }

            // Обновляем статус заявки на "Отклонено"
            tourRequest.Status = "Отклонено";

            _context.Update(tourRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Заявки текущего пользователя
        public async Task<IActionResult> UserRequests()
        {
            // Получаем Email текущего пользователя
            var userEmail = User.FindFirstValue(ClaimTypes.Name);

            if (userEmail == null)
            {
                return Unauthorized("Пользователь не авторизован.");
            }

            // Получаем UserId текущего пользователя
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            // Получаем все заявки текущего пользователя
            var userRequests = await _context.TourRequest
                .Include(tr => tr.Tour) // Включаем информацию о туре
                .Where(tr => tr.UserId == user.UserId)
                .ToListAsync();

            return View(userRequests);
        }
    }
}
