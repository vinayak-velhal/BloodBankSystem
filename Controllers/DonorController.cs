using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace BloodBankManagementSystem.Controllers
{
    public class DonorController : Controller
    {
        private readonly BloodBankContext db;

        public DonorController(BloodBankContext db)
        {
            this.db = db;
        }
        private static readonly List<SelectListItem> BloodGroupItems = new() {
           new SelectListItem("A+","A+"),
           new SelectListItem("A-","A-"),
           new SelectListItem("B+","B+"),
           new SelectListItem("B-","B-"),
           new SelectListItem("AB+","AB+"),
           new SelectListItem("AB-","AB-"),
           new SelectListItem("O+","O+"),
           new SelectListItem("O-","O-") 
        };
        public async Task<IActionResult> Index(string searchText)
        {
            var query = db.DonorRecords.AsQueryable();
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();
                query = query.Where(d =>
                    d.FullName.ToLower().Contains(searchText) ||
                    d.BloodGroup.ToLower().Contains(searchText) ||
                    d.DonationType.ToLower().Contains(searchText)
                );
            }
            var data = await query.ToListAsync();
            ViewBag.SearchText = searchText;
            return View(data);
        }
        public IActionResult Create()
        {
            ViewBag.DoctorList = db.DoctorInformations.Select(d => new { d.DoctorId, d.DoctorName }).ToList();
            ViewBag.PatientList = db.PatientRecords.Select(p => new { p.PatientId, Display = p.PatientName }).ToList();  //Patient Dropdown
            ViewBag.BloodGroups = new SelectList(BloodGroupItems, "Value", "Text");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonorRecord DR)
        {
          
            if (ModelState.IsValid)
            {
                await db.DonorRecords.AddAsync(DR);
                await db.SaveChangesAsync();
                TempData["Message"] = "<script>alert('Data Submitted Successfully')</script>";
                return RedirectToAction("Create", "Consent", new {donorId=DR.Id});

            }
            ViewBag.DoctorList = db.DoctorInformations.Select(d => new { d.DoctorId, d.DoctorName }).ToList();
            ViewBag.PatientList = db.PatientRecords.Select(p => new { p.PatientId, Display = p.PatientName }).ToList();
            ViewBag.BloodGroups = new SelectList(BloodGroupItems, "Value", "Text", DR.BloodGroup);
            return View(DR);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null | db.DonorRecords == null)
            {
                return NotFound();
            }
            var data = await db.DonorRecords.Include(d => d.PatientRecord).FirstOrDefaultAsync(d => d.Id == id); 
            if (data == null)
            {
                return NotFound();
            }
            ViewBag.DoctorList = db.DoctorInformations.Select(d => new{d.DoctorId,d.DoctorName}).ToList();
            ViewBag.PatientList = db.PatientRecords.Select(p => new { p.PatientId, Display = p.PatientName}).ToList();
            ViewBag.BloodGroups = new SelectList(BloodGroupItems, "Value", "Text", data.BloodGroup);
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, DonorRecord DR)
        {
            if (DR.Id != id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                db.DonorRecords.Update(DR);
                await db.SaveChangesAsync();
                TempData["Message"] = "<script>alert('Data Updated Successfully')</script>";
                return RedirectToAction("Index");

            }
            if (!ModelState.IsValid)
            {
                // Re-populate dropdowns
                ViewBag.PatientList = db.PatientRecords.Select(p => new { p.PatientId, Display = p.PatientName + " (" + p.HospitalName + ")" }).ToList();
                return View(DR);
            }

            ViewBag.BloodGroups = new SelectList(BloodGroupItems, "Value", "Text", DR.BloodGroup);
            return View(DR);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.DonorRecords == null)
            {
                return NotFound();
            }
            var data = await db.DonorRecords.Include(d=>d.PatientRecord).Include(d => d.DoctorInformation).FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                return NotFound();
            }
            var consent = await db.Consents.FirstOrDefaultAsync(c => c.DonorId == id);
            ViewBag.Consent = consent;
            return View(data);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null | db.DonorRecords == null)
            {
                return NotFound();
            }
            var data = await db.DonorRecords.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            var data = await db.DonorRecords.FindAsync(id);
            var consent = await db.Consents.FirstOrDefaultAsync(c=>c.Id==id);
            if (data != null)
            {
               db.DonorRecords.Remove(data);
            }
            if (consent != null)
            {
                db.Consents.Remove(consent);
            }
            await db.SaveChangesAsync();
            TempData["Message"] = "<script>alert('Data Deleted Successfully')</script>";
            return RedirectToAction("Index");
        }
       
    }
        
}
