using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Index()
        {
            return View(await _context.TourRequest.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
