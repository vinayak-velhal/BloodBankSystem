using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BloodBankManagementSystem.Models
{
    public class BloodRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual PatientRecord PatientRecord { get; set; }

        [Required]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Required]
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [Required]
        [Display(Name = "Requested Quantity (ml)")]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000 ml")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Display(Name = "Issue Date")]
        public DateTime? IssueDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending"; // Pending / Issued / Rejected

        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }

        // Foreign Key (option link to BloodInventory)
        [Display(Name = "Inventory ID (if issued)")]
        public int? InventoryId { get; set; }

        [ForeignKey("InventoryId")]
        public virtual BloodInventory? BloodInventory { get; set; }

        [Display(Name = "Hospital Name")]
        public string? HospitalName { get; set; }


    }
}
