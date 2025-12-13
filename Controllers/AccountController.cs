using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodBankManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly BloodBankContext db;

        public AccountController(BloodBankContext db)
        {
            this.db = db;
        }
        // GET: Register
        public IActionResult Register()
        {
            return View();
        }
        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already registered.");
                    return View(user);
                }

                db.Users.Add(user);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }
        
        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Enter email and password";
                return RedirectToAction("Login");
            }
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid email or password!";
                return RedirectToAction("Login");
            }
            HttpContext.Session.SetString("UserName", user.FullName);
            TempData["SuccessMessage"] = "Login successful!";
            return RedirectToAction("Index", "Dashboard");
        }

        // GET: Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Logged out successfully!";
            return RedirectToAction("Login");
        }
    }
}
