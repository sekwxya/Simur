using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourAgency.Data;
using TourAgency.Models;

namespace TourAgency.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticsController : Controller
    {
        private readonly AppDbContext _context;

        public StatisticsController(AppDbContext context)
        {
            _context = context;
        }

        // Статистика по странам отдыха
        public async Task<IActionResult> Index()
        {
            var statistics = await _context.Tour
                .GroupBy(t => t.Country)
                .Select(group => new CountryStatistics
                {
                    Country = group.Key,
                    TourCount = group.Count(),
                    Revenue = group.Average(t => t.Price)
                })
                .ToListAsync();

            return View(statistics);
        }
    }
}
