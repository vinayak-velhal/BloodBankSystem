using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BloodBankManagementSystem.Controllers
{
    public class ConsentController : Controller
    {
        private readonly BloodBankContext db;
        private readonly IConfiguration _configuration;

        public ConsentController(BloodBankContext db, IConfiguration configuration)
        {
            this.db = db;
            _configuration = configuration;
        }
        public IActionResult Create(int donorId)
        {
            var existingConsent = db.Consents.FirstOrDefault(c => c.DonorId == donorId);
            if (existingConsent!=null)
            {
                if (existingConsent.IsVerified)
                {
                    // Already verified → go directly to details
                    TempData["Message"] = "Consent already verified for this donor.";
                    return RedirectToAction("Details", new { id = existingConsent.Id });
                }
                else
                {
                    // Not verified yet → reopen existing record for correction
                    TempData["Message"] = "Consent found but not verified. Please verify it now.";
                    return View(existingConsent);
                }
            }
           
            var consent = new Consent
            {
                DonorId = donorId,
                ConsentGiven = "I hereby provide my consent for blood donation."
            };
            return View(consent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Consent consent)  //string otpInput
        {
            if (consent.DonorId==0 && TempData["DonorId"]!=null)
            {
                consent.DonorId = Convert.ToInt32(TempData["DonorId"]);
            }
            if (!ModelState.IsValid)
            {
                return View(consent);
            }
            try
            {
                if (string.IsNullOrWhiteSpace(consent.SignedBy) || !consent.SignedBy.Contains("@"))
                {
                    ModelState.AddModelError("SignedBy", "Please enter a valid email address.");
                    return View(consent);
                }
                var existingConsent = await db.Consents.FirstOrDefaultAsync(c => c.DonorId == consent.DonorId && c.IsVerified == false);
                if (existingConsent != null)
                {
                    existingConsent.ConsentGiven = consent.ConsentGiven;
                    existingConsent.SignedBy = consent.SignedBy;
                    existingConsent.CreatedAt = DateTime.Now;
                    existingConsent.SignatureHash = null;
                    existingConsent.IsVerified = false;
                    await db.SaveChangesAsync();
                    consent = existingConsent;
                }
                else
                {
                    await db.Consents.AddAsync(consent);
                    await db.SaveChangesAsync();
                }

                // 1. Generate OTP
                Random rnd = new Random();
                string otp = rnd.Next(100000, 999999).ToString();

                // 2. Save temporarily
                TempData["Otp"] = otp;
                TempData["OtpTime"] = DateTime.Now;
                TempData["ConsentId"] = consent.Id;

                System.Diagnostics.Debug.WriteLine($"Generated OTP: {otp}");
                System.Diagnostics.Debug.WriteLine($"SignedBy(email): {consent.SignedBy}");

                await SendEmailAsync(consent.SignedBy, otp);

                TempData["Message"] = "OTP has been sent to your email.";
                return RedirectToAction("Verify", new { id = consent.Id });
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in OTP process: " + ex.Message);
                ModelState.AddModelError("", "Unable to send OTP right now. Try again later.");
                return View(consent);

            }

        }

        //GET: Consent/Verify
        public async Task<ActionResult> Verify(int id)
        {
            var consent = await db.Consents.FindAsync(id);
            return View(consent);
        }

        //POST: Consent/Verify
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Verify(int id, string enteredOtp)
        {
            var consent = await db.Consents.FindAsync(id);
            var otp = TempData["Otp"]?.ToString();
            var otpTime = TempData["OtpTime"] as DateTime?;

            if (otpTime == null || DateTime.Now - otpTime > TimeSpan.FromMinutes(1))
            {
                ViewBag.Error = "OTP expired.Please request a new one";
                return View(consent);
            }

            if (consent != null && otp == enteredOtp)
            {
                consent.IsVerified = true;
                consent.SignatureHash = Guid.NewGuid().ToString();  //Unique hash
                await db.SaveChangesAsync();

                TempData["Message"] = "Consent verified successfully";
                return RedirectToAction("Details", new { id = consent.Id });
            }
            ViewBag.Error = "Invalid OTP.Please try again.";
            return View(consent);
        }

        public async Task<IActionResult> ResendOtp(int id)
        {
            var consent = await db.Consents.FindAsync(id);
            if (consent == null)
            {
                return NotFound();
            }
            Random rnd = new Random();
            string newOtp = rnd.Next(100000, 999999).ToString();

            TempData["Otp"] = newOtp;
            TempData["OtpTime"] = DateTime.Now;
            TempData["ConsentId"] = consent.Id;

            await SendEmailAsync(consent.SignedBy, newOtp);

            TempData["Message"] = "A new OTP has been sent to your email.";
            return RedirectToAction("Verify", new { id = consent.Id });
        }
        public async Task<IActionResult> Details(int id)
        {
            var data = await (from c in db.Consents join d in db.DonorRecords on c.DonorId equals d.Id where c.Id == id    //uses Inner join here like c.DonorId(which is foreign key of consent table)equal to d.Id(which is primary key of donor table)
                              select new
                              {
                                  Consent = c,
                                  DonorName = d.FullName
                              }).FirstOrDefaultAsync();
            
            if (data == null)
            {
                TempData["Message"] = "No consent record found for this donor.";
                return RedirectToAction("Create", new { donorId = id });
            }
            ViewBag.DonorName = data.DonorName;
            return View(data.Consent);
        }

        //Helper Methods
        private async Task SendEmailAsync(string email, string otp)
        {
            try
            {
                var fromAddress = new MailAddress("vinayakvelhal1@gmail.com", "Blood Bank OTP");
                var toAddress = new MailAddress(email);
                string subject = "Your Consent OTP";
                string body = $"Your OTP for consent verification is:{otp}";
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "smtp-relay.brevo.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_configuration["Smtp:User"],_configuration["Smtp:Pass"]);
                    smtp.Timeout = 20000;
                    await smtp.SendMailAsync(message);
                }
                System.Diagnostics.Debug.WriteLine("Email Sent Successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Email Error:" + ex.Message);
                throw;
            }
        }
    }
}

