 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BloodBankManagementSystem.Models;

namespace BloodBankManagementSystem.Controllers
{
    public class HospitalInformationsController : Controller
    {
        private readonly BloodBankContext _context;

        public HospitalInformationsController(BloodBankContext context)
        {
            _context = context;
        }

        // GET: HospitalInformations
        public async Task<IActionResult> Index(string searchText)
        {
            var query = _context.HospitalInformations.AsQueryable();
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();
                query = query.Where(h =>
                    h.HospitalName.ToLower().Contains(searchText)
                );
            }
            var data = await query.ToListAsync();
            ViewBag.SearchText = searchText;
            return View(data);
        }

        // GET: HospitalInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalInformation = await _context.HospitalInformations.FirstOrDefaultAsync(m => m.HospitalId == id);
            if (hospitalInformation == null)
            {
                return NotFound();
            }

            return View(hospitalInformation);
        }

        // GET: HospitalInformations/Create
            public IActionResult Create()
            {
                return View();
            
            }

        // POST: HospitalInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HospitalInformation model)
        {
            if (ModelState.IsValid)
            {
                _context.HospitalInformations.Add(model);
                await _context.SaveChangesAsync();

                // ✅ Return hospitalId to script
                return Json(new { success = true, hospitalId = model.HospitalId });
            }

            return Json(new { success = false });
        }

        // GET: HospitalInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalInformation = await _context.HospitalInformations.FindAsync(id);
            if (hospitalInformation == null)
            {
                return NotFound();
            }
            return View(hospitalInformation);
        }

        // POST: HospitalInformations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HospitalId,HospitalName,HospitalAddress,DoctorName,HospitalContact,PersonContact")] HospitalInformation hospitalInformation)
        {
            if (id != hospitalInformation.HospitalId)

            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hospitalInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalInformationExists(hospitalInformation.HospitalId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hospitalInformation);
        }

        // GET: HospitalInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalInformation = await _context.HospitalInformations.FirstOrDefaultAsync(m => m.HospitalId == id);
            
            if (hospitalInformation == null)
            {
                return NotFound();
            }

            return View(hospitalInformation);
        }

        // POST: HospitalInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospitalInformation = await _context.HospitalInformations.FindAsync(id);
            if (hospitalInformation != null)
            {
                _context.HospitalInformations.Remove(hospitalInformation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalInformationExists(int id)
        {
            return _context.HospitalInformations.Any(e => e.HospitalId == id);
        }
    }
}
