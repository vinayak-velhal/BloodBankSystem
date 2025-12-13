using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly BloodBankContext db;

        public DashboardController(BloodBankContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            // Check if user is logged in
            var userName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(userName))
            {
                TempData["ErrorMessage"] = "Please log in first.";
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = userName;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Logged out successfully!";
            return RedirectToAction("Login", "Account");
        }

        //Return Chart Data(AJAX JSON)
        public JsonResult GetBloodInventoryData()
        {
            var data = db.BloodInventories.GroupBy(b => new {b.BloodGroup,b.Status}).Select(g => new
            {
                BloodGroup = g.Key,
                TotalUnits = g.Sum(x => x.Quantity),
                Status=g.Key.Status
            }).ToList();

            return Json(data);
        }
    }
}
