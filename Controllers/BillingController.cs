using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore;

namespace BloodBankManagementSystem.Controllers
{
    public class BillingController : Controller
    {
        private readonly BloodBankContext db;

        public BillingController(BloodBankContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index(string searchText)
        {
            var query = db.Billings.Include(b => b.BloodRequest).OrderByDescending(b => b.BillDate).AsQueryable();
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.ToLower();

                query = query.Where(b =>
                    b.BloodRequest.PatientName.ToLower().Contains(searchText) ||     // search by patient name
                    b.BloodRequest.BloodGroup.ToLower().Contains(searchText) ||   // search by blood group
                    b.TransactionNo.ToLower().Contains(searchText) 
               );
            }
            ViewBag.SearchText = searchText;
            var data = await query.ToListAsync();
            return View(data);
        }
        public IActionResult Create(int? requestId)
        {
            // Fetch blood requests that are issued (for dropdown)
            var issuedRequests = db.BloodRequests
                .Where(r => r.Status == "Issued")
                .Select(r => new
                {
                    r.RequestId,
                    Display = r.PatientName + " - " + r.BloodGroup + " (" + r.RequestDate.ToString("dd-MMM-yyyy") + ")"
                })
                .ToList();

            ViewBag.RequestIssueList = issuedRequests;

            var model = new Billing();

            // Auto-fill if redirected from BloodRequest/Edit
            if (requestId != null)
            {
                var req = db.BloodRequests.FirstOrDefault(r => r.RequestId == requestId);
                if (req != null)
                {
                    model.RequestId = req.RequestId;
                    model.PatientName = req.PatientName;
                    model.BloodGroup = req.BloodGroup;
                    model.HospitalName = req.HospitalName;
                    model.Quantity = req.Quantity;
                }
            }

            return View(model);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Billing bill)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                foreach (var err in allErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Validation Error: " + err);
                }
            }
            if (ModelState.IsValid)
            {
                bill.TotalAmount = bill.Quantity * bill.RatePerMl;
                db.Billings.Add(bill);
                await db.SaveChangesAsync();

                TempData["SuccessMessage"] = "Billing record created successfully";

                // Redirect to Details page of the newly created bill
                return RedirectToAction("Details", new { id = bill.BillId });
            }

            ViewBag.RequestIssueList = db.BloodRequests
                .Where(r => r.Status == "Issued")
                .Select(r => new
                {
                    r.RequestId,
                    Display = r.RequestId + " - " + r.BloodGroup + " (" + r.PatientName + ")" + " (" + r.HospitalName + ")"
                })
                .ToList();

            return View(bill);
        }

        [HttpGet]
        public async Task<JsonResult> GetRequestIssueDetails(int id)
        {
            var details = await db.BloodRequests.Where(r => r.RequestId == id).Select(r => new{r.PatientName,r.BloodGroup,r.Quantity,r.HospitalName}).FirstOrDefaultAsync();

            if (details == null)
            {
                return Json(new { success = false });

            }
            return Json(new { success = true, data = details });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var bill = await db.Billings.Include(b => b.BloodRequest).FirstOrDefaultAsync(b => b.BillId == id);
            if (bill==null)
            {
                return NotFound();
            }
            return View(bill);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bill = await db.Billings.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }
            var bloodRequest = await db.BloodRequests.FirstOrDefaultAsync(r => r.RequestId == bill.RequestId);
            if (bloodRequest!=null)
            {
                ViewBag.PatientName = bloodRequest.PatientName;

            }
            ViewBag.RequestIssueList = db.BloodRequests.Where(r => r.Status == "Issued")
                .Select(r => new { r.RequestId, Display = r.RequestId + " - " + r.BloodGroup + r.PatientName }).ToList();
            return View(bill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Billing bill)
        {
            if (id !=bill.BillId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                bill.TotalAmount = bill.Quantity * bill.RatePerMl;
                db.Update(bill);
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Billing record updated successfully";
                return RedirectToAction("Index");
            }
            ViewBag.RequestIssueList = db.BloodRequests.Where(r => r.Status == "Issued").Select(r => new { r.RequestId, Display = r.RequestId + " - " + r.BloodGroup + " (" + r.PatientName + ")" }).ToList();
            return View(bill);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bill = await db.Billings.Include(r => r.BloodRequest).FirstOrDefaultAsync(b => b.BillId == id);
            if (bill == null)
            {
                return NotFound();
            }
            return View(bill);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bill = await db.Billings.FindAsync(id);
            if (bill!=null)
            {
                db.Billings.Remove(bill);
                await db.SaveChangesAsync();
            }
            TempData["SuccessMessage"] = "Billing record deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
