using Azure.Core;
using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BloodBankManagementSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly BloodBankContext db;

        public PatientController(BloodBankContext db)
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
        // inside PatientController
            [HttpGet]
            public async Task<IActionResult> GetPatientInfo(int id)
            {
                var patient = await db.PatientRecords.FindAsync(id);
                if (patient == null)
                {
                    return NotFound();
                }
                return Json(new { patientName = patient.PatientName, bloodGroup = patient.BloodGroup, hospitalName = patient.HospitalName });
            }

        public async Task<IActionResult> Index(string searchText)
        {
            var query = db.PatientRecords.AsQueryable();
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();
                query = query.Where(p =>
                    p.PatientName.ToLower().Contains(searchText) ||
                    p.DiseaseName.ToLower().Contains(searchText) ||
                    p.BloodGroup.ToLower().Contains(searchText)
                );
            }
            var data = await query.ToListAsync();
            ViewBag.SearchText = searchText;
            return View(data);
        }
        public IActionResult Create(int hospitalId)
        {
            var model = new PatientRecord();

            if(hospitalId > 0)
            {
                var hospital = db.HospitalInformations.FirstOrDefault(h => h.HospitalId == hospitalId);
                if (hospitalId != null)
                {
                    model.HospitalId = hospital.HospitalId;
                    model.HospitalName = hospital.HospitalName;
                }
            }
            ViewBag.BloodGroups = new SelectList(BloodGroupItems, "Value", "Text");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientRecord patientRecord)
        {
            try 
            {
                System.Diagnostics.Debug.WriteLine($"Patient Create called at {DateTime.UtcNow:O}");
                System.Diagnostics.Debug.WriteLine("HospitalInformation is null? : " + (patientRecord.HospitalInformation == null));

                if (ModelState.IsValid)
                {
                    await db.PatientRecords.AddAsync(patientRecord);
                    await db.SaveChangesAsync();
                        var script = $@"
                        <!doctype html>
                        <html><head><meta charset='utf-8'></head><body>
                        <script>
                        if (window.parent) 
                        {{
                        window.parent.postMessage({{type: 'PatientSaved', patientId: {patientRecord.PatientId}}},'*');
                        }}
                        </script>
                        </body></html>";
                    System.Diagnostics.Debug.WriteLine($"DEBUG: Newly saved patientId = {patientRecord.PatientId}");
                    return Json(new { success = true, patientId = patientRecord.PatientId });                    
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("DEBUG: ModelState invalid");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        System.Diagnostics.Debug.WriteLine($"Model Error: {error.ErrorMessage}");
                    }
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
            }
            ViewBag.BloodGroups = new SelectList(BloodGroupItems, "Value", "Text", patientRecord.BloodGroup);
            return Json(new {success=false});
        } 
        public async Task<IActionResult> Edit(int? id)
        {
            var patient = db.PatientRecords.Include(p => p.HospitalInformation).FirstOrDefault(p => p.PatientId == id);
            if (id==null | db.PatientRecords==null)
            {
                return NotFound();
            }
            if (patient==null)
            {
                return NotFound();
            }
            System.Diagnostics.Debug.WriteLine("Hospital name; "+ patient.HospitalInformation?.HospitalName);
            ViewBag.Hospitals = db.HospitalInformations.Select(h => new { h.HospitalId, h.HospitalName }).ToList();
            ViewBag.BloodGroups = new SelectList(BloodGroupItems, "Value", "Text", patient.BloodGroup);
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, PatientRecord patientRecord)
        {
            if (id != patientRecord.PatientId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                 
                db.PatientRecords.Update(patientRecord);
                await db.SaveChangesAsync();
                TempData["Message"] = "<script>alert('Data Updated successfully')</script>";
                return RedirectToAction("Index");
            }
            ViewBag.BloodGroups = new SelectList(BloodGroupItems,"Value","Text",patientRecord.BloodGroup);
            return View(patientRecord);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null | db.PatientRecords == null)
            {
                return NotFound();
            }
            var patient = await db.PatientRecords.FirstOrDefaultAsync(x => x.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null | db.PatientRecords == null)
            {
                return NotFound();
            }
            var patient = await db.PatientRecords.FirstOrDefaultAsync(x => x.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
           
            bool requestExists = await db.BloodRequests.AnyAsync(b => b.PatientId == id);
            bool donorExists = await db.DonorRecords.AnyAsync(d => d.PatientId == id);

            if (requestExists && donorExists)
            {
                TempData["Errormsg"] = "<script>alert('Cannot delete this record. 1.Please delete the request from home->blood requests. 2.Please delete the donor from home->make donors.')</script>";
                return RedirectToAction("Index");
            }
            else if (requestExists)
            {
                TempData["Errormsg"] = "<script>alert('Cannot delete this record. Please delete the request first from home->blood requests.')</script>";
                return RedirectToAction("Index");
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }
                var patient = await db.PatientRecords.FindAsync(id);
                var hospital = await db.HospitalInformations.FirstOrDefaultAsync(h => h.HospitalId == id);
                if (patient == null)
                {
                    return NotFound();
                }
                db.PatientRecords.Remove(patient);
                if (hospital != null)
                {
                    db.HospitalInformations.Remove(hospital);
                }
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Patient and corresponding hospital record deleted successfully";
                return RedirectToAction("Index");
            }
            
        }
    }
}
