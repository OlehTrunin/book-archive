using System.Security.Claims;
using BookArchive.Models;
using BookArchive.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookArchive.Controllers
{
    public class AuthController : Controller
    { 
        private readonly DataContext _context;

        public AuthController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                // Verify password (consider password hashing)
                if (user != null && user.Password == model.Password) // Replace with hash comparison
                {
                    await SignInUser(user); // Sign in user with claims
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Неправильні логін чи пароль");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if user already exists
                var existingUser = await _context.Users.AnyAsync(u => u.Email == model.Email);
                if (existingUser)
                {
                    ModelState.AddModelError("", "Цей email вже зареєстрований");
                    return View(model);
                }

                // Create user and assign default "user" role
                var userRole = await _context.Roles.SingleOrDefaultAsync(r => r.Name == "user") 
                               ?? new Role { Name = "user" };

                var user = new User 
                { 
                    Email = model.Email, 
                    Password = model.Password, // Password should ideally be hashed
                    Role = userRole
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await SignInUser(user);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        private async Task SignInUser(User user)
        {
            // Check for null user or null role
            if (user == null || user.Role == null) 
                throw new ArgumentNullException(nameof(user), "User or User Role cannot be null");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                claimsPrincipal);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return Content("Недостатньо прав");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}
