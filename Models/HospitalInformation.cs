using System.ComponentModel.DataAnnotations;

namespace BloodBankManagementSystem.Models
{
    public class HospitalInformation
    {
        [Key]
        [Display(Name = "Hospital Id")]
        public int HospitalId { get; set; }

        [Required(ErrorMessage ="Hospital Name is Mandentory")]
        [Display(Name="Hospital Name")]
        public string? HospitalName { get; set; }

        [Required(ErrorMessage = "Hospital Address is Madentory")]
        [Display(Name = "Hospital Address")]
        public string? HospitalAddress { get; set; }

        [Required(ErrorMessage = "Doctor Name is Madentory")]
        [Display(Name = "Doctor Name")]
        public string? DoctorName { get; set; }

        [Required(ErrorMessage = "Hospital Contact Number is Madentory")]
        [Display(Name = "Hospital Contact")]
        public string? HospitalContact { get; set; }

        [Required(ErrorMessage = "Person Contact Number is Madentory")]
        [Display(Name = "Person Contact")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit mobile number.")]
        public string? PersonContact { get; set; }

    }
}
