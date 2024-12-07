using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourAgency.Data;
using TourAgency.Models;

namespace TourAgency.Controllers
{
    public class TourController:Controller
    {
        private readonly AppDbContext dbContext;

        public TourController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tour tour)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(tour);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tour);
        }
    }
}
