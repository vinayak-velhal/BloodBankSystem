using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BloodBankManagementSystem.Controllers
{
    public class DoctorInformationsController : Controller
    {
        private readonly BloodBankContext db;

        public DoctorInformationsController(BloodBankContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index(string searchText)
        {
            var query = db.DoctorInformations.AsQueryable();
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();
                query = query.Where(d =>
                    d.DoctorName.ToLower().Contains(searchText) ||
                    d.Qualification.ToLower().Contains(searchText)
                );
            }
            var data = await query.ToListAsync();
            ViewBag.SearchText = searchText;
            //var data = await db.DoctorInformations.ToListAsync();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorInformation DC)
        {
            if (ModelState.IsValid)
            {
                await db.DoctorInformations.AddAsync(DC);
                await db.SaveChangesAsync();
                TempData["InsertMsg"] = "<script>alert('Data inserted successfully')</script>";
                return RedirectToAction("Index");
            }
            return View(DC);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id==null | db.DoctorInformations==null)
            {
                return NotFound();
            }
            var data = await db.DoctorInformations.FindAsync(id);
            if (data==null)
            {
                return NotFound();
            }

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,DoctorInformation DC)
        {
            if (id!=DC.DoctorId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                db.Update(DC);
                await db.SaveChangesAsync();
                TempData["InsertMsg"] = "<script>alert('Data updated successfully')</script>";
                return RedirectToAction("Index");
            }
            return View(DC);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null | db.DoctorInformations == null)
            {
                return NotFound();
            }
            var data = await db.DoctorInformations.FirstOrDefaultAsync(x => x.DoctorId == id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null | db.DoctorInformations == null)
            {
                return NotFound();
            }
            var data = await db.DoctorInformations.FirstOrDefaultAsync(x => x.DoctorId == id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            var data = await db.DoctorInformations.FindAsync(id);
            if (data==null)
            {
                return NotFound();
            }
            bool donorExists = await db.DonorRecords.AnyAsync(d=> d.DoctorId == id);
            if (donorExists)
            {
                TempData["Errormsg"] = "<script>alert('Cannot delete this record. Please delete the donor first from Make Donors section.')</script>";
                return RedirectToAction("Index");
            }

            db.DoctorInformations.Remove(data);
            await db.SaveChangesAsync();
            TempData["InsertMsg"] = "<script>alert('Data updated successfully')</script>";
            return RedirectToAction("Index");
            
        }

    }
}

