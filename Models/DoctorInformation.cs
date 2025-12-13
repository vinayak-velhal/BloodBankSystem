using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBankManagementSystem.Models
{
    public class DoctorInformation
    {
        [Key]
        [Display(Name ="Doctor Id")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage ="Doctor Name is Mandentory")]
        [Display(Name = "Doctor Name")]
        [Column(TypeName ="varchar(50)")]
        public string? DoctorName { get; set; }

        [Required(ErrorMessage = "Mobile No is Mandentory")]
        [Display(Name = "Mobile No")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10,MinimumLength =10,ErrorMessage ="Mobile No. Must be 10-digit")]
        public string? MobileNo { get; set; }


        [Required(ErrorMessage = "Doctor Address is Mandentory")]
        [Display(Name = "Doctor Address")]
        [Column(TypeName = "varchar(100)")]
        public string? DoctorAddress { get; set; }

        [Required(ErrorMessage = "Qualification is Mandentory")]
        [Display(Name = "Qualification")]
        [Column(TypeName = "varchar(50)")]
        public string? Qualification { get; set; }

        [Required(ErrorMessage = "Email Id is Mandentory")]
        [Display(Name = "Email Id")]
        [DataType(DataType.EmailAddress)]
        public string? EmailId { get; set; }
    }
}
