using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBankManagementSystem.Models
{
    public class PatientRecord
    {
        [Key]
        [Display(Name = "Patient Id")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Patient Name is Mandentory")]
        [Display(Name = "Patient Name")]
        [Column(TypeName = "varchar(50)")]
        public string? PatientName { get; set; }

        [Required(ErrorMessage = "Patient Address is Mandentory")]
        [Display(Name = "Patient Address")]
        [Column(TypeName = "varchar(100)")]
        public string? PatientAddress { get; set; }

        [Required(ErrorMessage = "Mobile No is Mandentory")]
        [Display(Name = "Mobile No")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile No. Must be 10-digit")]
        public string? MobileNo { get; set; }

        [Required(ErrorMessage = "Disease Name is Mandentory")]
        [Display(Name = "Disease Name")]
        [Column(TypeName = "varchar(50)")]
        public string? DiseaseName { get; set; }

        [Required(ErrorMessage = "Blood Group is Mandentory")]
        [Column(TypeName = "varchar(50)")]
        public string? BloodGroup { get; set; }

        [Required(ErrorMessage = "Hospital Name is Mandentory")]
        [Display(Name = "Hospital Name")]
        [Column(TypeName = "varchar(50)")]
        public string? HospitalName { get; set; }

        // Foreign Key (option link to BloodInventory)
        [Display(Name = "Hospital ID")]
        public int? HospitalId { get; set; }

        [ForeignKey("HospitalId")]
        public virtual HospitalInformation? HospitalInformation { get; set; }
        public int? BillId { get; set; }

        [ForeignKey("BillId")]
        public virtual Billing? Billing { get; set; }


    }
}
