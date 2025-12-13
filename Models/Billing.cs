using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBankManagementSystem.Models
{
    public class Billing
    {
        [Key]
        public int BillId { get; set; }

        [Required]
        [Display(Name = "Bill Date")]
        public DateTime BillDate { get; set; } = DateTime.Now;

        [Display(Name = "Patient Name")]
        [ValidateNever]
        public string? PatientName { get; set; }

        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [Display(Name = "Quantity (ml)")]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000 ml")]
        public int Quantity { get; set; }

        [Display(Name = "Rate per ml (₹)")]
        [Range(1, 500, ErrorMessage = "Enter valid rate")]
        public decimal RatePerMl { get; set; }

        [Display(Name = "Total Amount (₹)")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Select a payment mode")]
        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; }

        [Display(Name = "Transaction/Receipt No.")]
        public string TransactionNo { get; set; }

        [Display(Name = "Request ID")]
        public int? RequestId { get; set; }

        [ForeignKey("RequestId")]
        [ValidateNever]
        public virtual BloodRequest? BloodRequest { get; set; }

        [Display(Name = "Hospital Name")]
        public string HospitalName { get; set; }

    }
}
