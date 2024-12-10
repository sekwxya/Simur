using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourAgency.Data;
using TourAgency.Models;

namespace TourAgency.Controllers
{
    public class ToursController : Controller
    {
        private readonly AppDbContext _context;

        public ToursController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Tours
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Tour.Include(t => t.Discount);
            return View(await appDbContext.ToListAsync());
        }

        //public IActionResult Discount()
        //{
        //    ViewData["DiscountId"] = new SelectList(_context.Discount, "DiscountId", "Name");
        //    return View();
        //}

        // GET: Tours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tour
                .Include(t => t.Discount)
                .FirstOrDefaultAsync(m => m.TourId == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // GET: Tours/Create
        public IActionResult Create()
        {
            ViewData["DiscountId"] = new SelectList(_context.Discount, "DiscountId", "Name");
            return View();
        }

        // POST: Tours/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TourId,Title,Description,Country,Price,StartDate,EndDate,DiscountId")] Tour tour)
        {
            tour.Discount = _context.Discount.Where(x => x.DiscountId == tour.DiscountId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                _context.Add(tour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountId"] = new SelectList(_context.Discount, "DiscountId", "Name", tour.DiscountId);
            return View(tour);
        }

        // GET: Tours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tour.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            ViewData["DiscountId"] = new SelectList(_context.Discount, "DiscountId", "DiscountId", tour.DiscountId);
            return View(tour);
        }

        // POST: Tours/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TourId,Title,Description,Country,Price,StartDate,EndDate,DiscountId")] Tour tour)
        {
            if (id != tour.TourId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(tour.TourId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountId"] = new SelectList(_context.Discount, "DiscountId", "DiscountId", tour.DiscountId);
            return View(tour);
        }

        // GET: Tours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tour
                .Include(t => t.Discount)
                .FirstOrDefaultAsync(m => m.TourId == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tour = await _context.Tour.FindAsync(id);
            if (tour != null)
            {
                _context.Tour.Remove(tour);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TourExists(int id)
        {
            return _context.Tour.Any(e => e.TourId == id);
        }
    }
}
