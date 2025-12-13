using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBankManagementSystem.Models
{
    public class Consent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DonorId { get; set; }  //FK to Donor
        //--Consent
        [Required(ErrorMessage = "Consent is Mandentory")]
        [Display(Name = "Consent")]
        public string ConsentGiven { get; set; }

        //Digital signature fields
        public string? SignatureHash { get; set; } //Unique hash after OTP verified
 
        [Required]
        public string SignedBy { get; set; }   //Donor's mobile/emailid

        public bool IsVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now; //Timestamp of signing

        [ForeignKey("DonorId")]
        public virtual DonorRecord? Donor { get; set; }
    }
}
