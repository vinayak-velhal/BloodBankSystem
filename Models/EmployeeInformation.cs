using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBankManagementSystem.Models
{
    public class EmployeeInformation
    {
        [Key]
        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee Name is Mandentory")]
        [Display(Name = "Employee Name")]
        [Column(TypeName = "varchar(50)")]
        public string? EmployeeName { get; set; }

        [Required(ErrorMessage = "Mobile No is Mandentory")]
        [Display(Name = "Mobile No")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile No. Must be 10-digit")]
        public string? MobileNo { get; set; }

        [Required(ErrorMessage = "Employee Address is Mandentory")]
        [Display(Name = "Employee Address")]
        [Column(TypeName = "varchar(100)")]
        public string? EmployeeAddress { get; set; }

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
