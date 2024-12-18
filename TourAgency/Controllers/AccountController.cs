using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using TourAgency.Models;
using System.Security.Claims;
using TourAgency.Data;
using Microsoft.EntityFrameworkCore;

namespace TourAgency.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User userDto)
        {
            var userCheck = await _context.User.FirstOrDefaultAsync(x => x.Email == userDto.Email);
            if (userCheck == null)
            {
                TempData["error"] = "Incorrect email or password";
                return View("Login");
            }
            if (userDto.Password != userCheck.Password)
            {
                TempData["error"] = "Incorrect email or password";
                return View("Login");
            }

            var claim = Authenticate(userCheck);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claim));
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User userDto)
        {
            var userCheck = await _context.User.FirstOrDefaultAsync(x => x.Email == userDto.Email);
            if (userCheck != null)
            {
                TempData["error"] = "User with this email already exists";
                return View("Register");
            }
            userDto.Role = "Default";
            await _context.AddAsync(userDto);
            var result = _context.SaveChanges();
            if (result < 0)
            {
                TempData["error"] = "Something went wrong while saving";
                return View("Register");
            }
            var claim = Authenticate(userDto);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claim));
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>()
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
        };
            return new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }

}