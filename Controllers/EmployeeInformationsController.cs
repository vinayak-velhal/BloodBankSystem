using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BloodBankManagementSystem.Controllers
{
    public class EmployeeInformationsController : Controller
    {
        private readonly BloodBankContext db;

        public EmployeeInformationsController(BloodBankContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index(string searchText)
        {
            var query = db.EmployeeInformations.AsQueryable();
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();
                query = query.Where(d =>
                    d.EmployeeName.ToLower().Contains(searchText) ||
                    d.Qualification.ToLower().Contains(searchText)
                );
            }
            var data = await query.ToListAsync();
            ViewBag.SearchText = searchText;
            //var data= await db.EmployeeInformations.ToListAsync();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeInformation emp)
        {
            if (ModelState.IsValid)
            {
               await db.EmployeeInformations.AddAsync(emp);
               await db.SaveChangesAsync();
                TempData["Message"] = "<script>alert('Data Submitted')</script>";
                return RedirectToAction("Index");
            }
            return View(emp);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id==null | db.EmployeeInformations==null)
            {
                return NotFound();
            }
            var data = await db.EmployeeInformations.FindAsync(id);
            if (data==null)
            {
                return NotFound();
            }
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,EmployeeInformation emp)
        {
            if (id!=emp.EmployeeId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                db.EmployeeInformations.Update(emp);
                await db.SaveChangesAsync();
                TempData["Message"] = "<script>alert('Data Updated')</script>";
                return RedirectToAction("Index");
            }
            return View(emp);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null | db.EmployeeInformations == null)
            {
                return NotFound();
            }
            var data = await db.EmployeeInformations.FirstOrDefaultAsync(x=>x.EmployeeId==id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null | db.EmployeeInformations == null)
            {
                return NotFound();
            }
            var data = await db.EmployeeInformations.FirstOrDefaultAsync(x => x.EmployeeId == id);
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
            var data = await db.EmployeeInformations.FindAsync(id);
            if (data!=null)
            {
                db.EmployeeInformations.Remove(data);
            }
            await db.SaveChangesAsync();
            TempData["Message"] = "<script>alert('Data Deleted')</script>";
            return RedirectToAction("Index");
            
        }
    }
}
