using Azure.Core;
using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodBankManagementSystem.Controllers
{
    public class BloodRequestController : Controller
    {
        private readonly BloodBankContext db;

        public BloodRequestController(BloodBankContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index(string searchText)
        {
            var query = db.BloodRequests.Include(r => r.BloodInventory).Include(r => r.PatientRecord).OrderByDescending(r => r.RequestDate).AsQueryable();    /*ToListAsync();*/
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();

                query = query.Where(r =>
                    r.PatientRecord.PatientName.ToLower().Contains(searchText) ||     // search by patient name
                    r.PatientRecord.BloodGroup.ToLower().Contains(searchText)        // search by blood group  
               );
            }

            ViewBag.SearchText = searchText;
            var data = await query.ToListAsync();
            return View(data);
        }
        public IActionResult Create(int? patientId)
        {
            var patientinfo=db.PatientRecords.Select(p => new{p.PatientId,Display = p.PatientName}).ToList();
            ViewBag.PatientList = patientinfo;

            var model = new BloodRequest();

            // Auto-fill if redirected from BloodRequest/Edit
            if (patientId != null)
            {
                var req = db.PatientRecords.FirstOrDefault(p => p.PatientId == patientId);
                if (req != null)
                {
                    model.PatientId = req.PatientId;
                    model.PatientName = req.PatientName;
                    model.BloodGroup = req.BloodGroup;
                    model.HospitalName = req.HospitalName;
                }
            }

            return View(model);
        }
        [HttpGet]
        public JsonResult GetPatientDetails(int id)
        {
            var patient = db.PatientRecords.Where(p => p.PatientId == id).Select(p => new {p.PatientName, p.BloodGroup, p.HospitalName }).FirstOrDefault();
            return Json(patient);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BloodRequest request)
        {
            if (ModelState.IsValid)
            {
                // Re-populate dropdowns
                ViewBag.PatientList = db.PatientRecords.Select(p => new { p.PatientId, Display = p.PatientName + "(" + p.BloodGroup + ")" + "(" + p.HospitalName + ")" }).ToList();
                return View(request);
            }
            // Auto-fill patient details from master
            var patient = await db.PatientRecords.FindAsync(request.PatientId);
            if (patient != null)
            {
                request.PatientName = patient.PatientName;
                request.BloodGroup = patient.BloodGroup;
                request.HospitalName = patient.HospitalName;
            }

            // Ensure initial status is Pending
            request.Status = "Pending";

            // Initialize remarks if empty
            if (string.IsNullOrWhiteSpace(request.Remarks))
            {
                request.Remarks = $"{DateTime.Now:yyyy-MM-dd HH:mm} – Request for blood";
            }
            else
            {
                request.Remarks = $"{DateTime.Now:yyyy-MM-dd HH:mm} – {request.Remarks}";
            }
             db.BloodRequests.Add(request);
             await db.SaveChangesAsync();
             TempData["SuccessMessage"] = "Blood Request Submitted Successfully";
             return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await db.BloodRequests.FindAsync(id.Value);
            if (request == null)
            {
                return NotFound();
            }

            // Populate dropdowns
            ViewBag.PatientList = db.PatientRecords.Select(p => new { p.PatientId, Display = p.PatientName + " (" + p.BloodGroup + ")" }).ToList();
            ViewBag.InventoryList = db.BloodInventories.Where(b => b.Status == "Available" || b.InventoryId == request.InventoryId).Select(b => new{b.InventoryId,Display = /*b.InventoryId + " - " + */b.BloodGroup /*+ " (Exp: " + b.ExpiryDate.ToString("dd-MMM-yyyy") + ")"*/}).ToList();
             return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BloodRequest model, string? NewRemark)
        {
            System.Diagnostics.Debug.WriteLine("=== POST Edit STARTED ===");
            System.Diagnostics.Debug.WriteLine($"RequestId = {model.RequestId}, InventoryId = {model.InventoryId}");

            var existing = await db.BloodRequests.FindAsync(model.RequestId);
            if (existing == null)
            {
                System.Diagnostics.Debug.WriteLine("Existing request not found!");
                return NotFound();
            }

            // Update editable fields
            existing.Quantity = model.Quantity;
            existing.InventoryId = model.InventoryId;

            // Handle Issued or Pending/Rejected
            if (model.InventoryId != null)
            {
                existing.Status = "Issued";
                existing.IssueDate = DateTime.Now;

                string autoRemark = $"{DateTime.Now:yyyy-MM-dd HH:mm} – Blood issued successfully.";
                existing.Remarks = string.IsNullOrWhiteSpace(existing.Remarks)
                    ? autoRemark
                    : existing.Remarks + "\n" + autoRemark;
            }
            else
            {
                existing.Status = "Rejected"; // keep pending if no inventory selected
                string autoRemark = $"{DateTime.Now:yyyy-MM-dd HH:mm} – Required quantity not available in stock.";
                existing.Remarks = string.IsNullOrWhiteSpace(existing.Remarks)
                    ? autoRemark
                    : existing.Remarks + "\n" + autoRemark;
            }

            // Append manual remark
            if (!string.IsNullOrWhiteSpace(NewRemark))
            {
                string remark = $"{DateTime.Now:yyyy-MM-dd HH:mm} – {NewRemark}";
                existing.Remarks += "\n" + remark;
            }

            db.BloodRequests.Update(existing);
            await db.SaveChangesAsync();

            System.Diagnostics.Debug.WriteLine($"Final Status = {existing.Status}");
            System.Diagnostics.Debug.WriteLine("=== POST Edit END ===");

            // Redirect conditions
            if (existing.Status == "Issued")
            {
                TempData["SuccessMessage"] = "Blood issued successfully. Proceed to billing.";
                System.Diagnostics.Debug.WriteLine(">>> Redirecting to Billing/Create...");
                return RedirectToAction("Create", "Billing", new { requestId = existing.RequestId });
            }

            TempData["SuccessMessage"] = "Request updated successfully.";
            System.Diagnostics.Debug.WriteLine(">>> Redirecting to BloodRequest/Index...");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var request = await db.BloodRequests.Include(r=>r.BloodInventory).Include(r => r.PatientRecord).FirstOrDefaultAsync(r=>r.RequestId==id.Value);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var request = await db.BloodRequests.Include(r => r.PatientRecord).FirstOrDefaultAsync(r => r.RequestId == id.Value);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            var request = await db.BloodRequests.FindAsync(id);
            if (request==null)
            {
                return NotFound();
            }
            bool billExists = await db.Billings.AnyAsync(b => b.RequestId == id);
            if (billExists)
            {
                TempData["Errormsg"] = "<script>alert('Cannot delete this record. Please delete the bill first from about Us -> Bill.')</script>";
                return RedirectToAction("Index");
            }
            db.BloodRequests.Remove(request);
            await db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Blood Request Deleted";
            return RedirectToAction("Index");
        }
    }
}
