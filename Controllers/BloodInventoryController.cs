using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using System.Linq.Expressions;

namespace BloodBankManagementSystem.Controllers
{
    public class BloodInventoryController : Controller
    {
        private readonly BloodBankContext db;

        public BloodInventoryController(BloodBankContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index(string? flow,string searchBloodGroup, string sortOrder)
        {

                ViewBag.Flow = flow;

                var inventoryQuery = db.BloodInventories.Include(b => b.Donor).AsQueryable();

                // Expire auto-check
                var expiredUnits = await db.BloodInventories.Where(b => b.ExpiryDate < DateTime.Now && b.Status != "Expired").ToListAsync();

                foreach (var unit in expiredUnits)
                    unit.Status = "Expired";

                if (expiredUnits.Any())
                    await db.SaveChangesAsync();

                // Search filter
                if (!string.IsNullOrEmpty(searchBloodGroup))
                    inventoryQuery = inventoryQuery.Where(b => b.BloodGroup.Contains(searchBloodGroup));

                // Sorting logic
                inventoryQuery = sortOrder switch
                {
                    "expiryAsc" => inventoryQuery.OrderBy(b => b.ExpiryDate),
                    "expiryDesc" => inventoryQuery.OrderByDescending(b => b.ExpiryDate),
                    _ => inventoryQuery.OrderByDescending(b => b.CollectionDate)
                };

                var inventory = await inventoryQuery.ToListAsync();
                return View(inventory);
        }

        

        public IActionResult Create()
        {
            ViewBag.Donors = new SelectList(db.DonorRecords, "Id", "FullName");
            var inventory = new BloodInventory
            {
                CollectionDate = DateTime.Now
            };
            return View(inventory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BloodInventory inventory)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach(var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            if (ModelState.IsValid)
            {
                inventory.Donor = null;
                db.BloodInventories.Add(inventory);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Blood unit added to inventory successfully.";
                return RedirectToAction("Index");
            }
            ViewBag.Donors = new SelectList(db.DonorRecords, "Id", "FullName");
            return View(inventory);
        }
        [HttpGet]
        public IActionResult GetDonorBloodGroup(int id)
        {
            // Try to find the donor record with the given ID
            var donor = db.DonorRecords.FirstOrDefault(d => d.Id == id);

            // If donor not found, return an empty bloodGroup JSON object
            if (donor == null)
            {
                return Json(new { bloodGroup = "", quantity = "" });
               
            }
            else
            {
                // If donor found, return their blood group
                return Json(new { bloodGroup = donor.BloodGroup, quantity = donor.Quantity });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var inventory = await db.BloodInventories.Include(b => b.Donor).FirstOrDefaultAsync(b => b.InventoryId == id);
            if (inventory==null)
            {
                return NotFound();
            }

            return View(inventory);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var inventory = await db.BloodInventories.Include(b => b.Donor).FirstOrDefaultAsync(b => b.InventoryId == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BloodInventory inventory)
        {
            if (id != inventory.InventoryId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existing = db.BloodInventories.Include(b => b.Donor).FirstOrDefault(b => b.InventoryId == id);

                if (existing == null)
                {
                    return NotFound();
                }
                // 1️⃣ Update editable fields
                existing.Quantity = inventory.Quantity;

                // 2️⃣ Auto-update status based on quantity or expiry
                if (existing.Quantity == 0)
                {
                    existing.Status = "Issued";
                }
                else if (existing.ExpiryDate < DateTime.Now)
                {
                    existing.Status = "Expired";
                }
                else
                {
                    existing.Status = "Available";
                }

                // 3️⃣ Save changes
                db.Update(existing);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Blood inventory updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var inventory = await db.BloodInventories.Include(b => b.Donor).FirstOrDefaultAsync(b => b.InventoryId == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>DeleteConfirm(int id)
        {
            var inventory = await db.BloodInventories.FindAsync(id);
            if (inventory !=null)
            {
                db.BloodInventories.Remove(inventory);
                await db.SaveChangesAsync();
            }
            TempData["SuccessMessage"] = "Blood inventory record deleted.";
            return RedirectToAction("Index");
        }
    }
}
