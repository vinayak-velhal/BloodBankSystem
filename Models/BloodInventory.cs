using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBankManagementSystem.Models
{
    public class BloodInventory
    {
        [Key]
        public int InventoryId { get; set; }

        [Required(ErrorMessage = "Blood Group is Mandentory")]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [Required(ErrorMessage = "Quantity is Mandentory")]
        [Range(0,1000,ErrorMessage ="Quantity must be between 0 and 1000 ml")]
        [Display(Name = "Quantity (in ml)")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Collection Date is Mandentory")]
        [Display(Name = "Collection Date")]
        public DateTime CollectionDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Expiry Date is Mandentory")]
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Donor ID")]
        public int? DonorId { get; set; }

        [ForeignKey("DonorId")]
        public virtual DonorRecord? Donor { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Available";

    }
}
