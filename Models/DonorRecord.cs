using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBankManagementSystem.Models
{
    public class DonorRecord:IValidatableObject
    {
        [Key]
        [Display(Name = "Donor Id")]
        public int Id { get; set; }

        //--Donor Information
        [Required(ErrorMessage = "Donor Name is Mandentory")]
        [Display(Name = "Donor Name")]
        [Column(TypeName = "varchar(50)")]
        public string FullName { get; set; }

        [Display(Name = "Father/Husband Name")]
        [Column(TypeName = "varchar(50)")]
        public string RelativeName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Donor Gender is Mandentory")]
        [Column(TypeName = "varchar(10)")]
        public string Gender { get; set; }
        public string? Occupation { get; set; }

        [Required(ErrorMessage = "Donor Address is Mandentory")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Donor Mobile no. is Mandentory")]
        [Display(Name = "Mobile No")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile No. Must be 10-digit")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Donation Type is Mandentory")]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Type of Donor")]
        public string DonationType { get; set; }

        [Required(ErrorMessage = "Blood Group is Mandentory")]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [Display(Name = "Donation Date")]
        public DateTime DonationDate { get; set; }

        [Display(Name = "Previous Donation Date")]
        public DateTime? LastDonationDate { get; set; }

        [Required]
        [Display(Name = "Requested Quantity (ml)")]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000 ml")]
        public int Quantity { get; set; }

        // Foreign Key (option link to BloodInventory)
        [Display(Name = "Doctor ID")]
        public int? DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual DoctorInformation? DoctorInformation { get; set; }

        [Display(Name = "Doctor's Name")]
        public string? DoctorName { get; set; }

        public virtual Consent? Consent { get; set; }


        //--Replacement Donor Section
        [Display(Name = "Patient")]
        public int? PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual PatientRecord? PatientRecord { get; set; }

        [Display(Name = "Hospital Name")] 
        public string? HospitalName { get; set; }

        [Display(Name = "Ward Number")]
        public string? WardNumber { get; set; }

        //--Medical Questionnaire Section
        [Required(ErrorMessage = "Please select: Did you have something to eat in last 04Hrs?")]
        public bool AteRecently { get; set; }

        [Required(ErrorMessage = "Please select: Are you feeling healthy today?")]
        public bool FillingWell { get; set; }

        [Required(ErrorMessage = "Please select: Have you had any fever in last 2 weeks?")]
        public bool HadFever { get; set; }

        [Required(ErrorMessage = "Please select: Are you under any medical treatment at present?")]
        public bool UnderTreatment { get; set; }

        [Required(ErrorMessage = "Please select: Have you undergone any surgery in the last 06 months?")]
        public bool RecentSurgery { get; set; }

        [Required(ErrorMessage = "Please select: Have you taken any injectable antibiotics in the last 4 days?")]
        public bool UsedAntibiotics { get; set; }

        [Required(ErrorMessage = "Please select: Have you received any blood transfusion in the last 06 months?")]
        public bool BloodTransfusion { get; set; }

        [Required(ErrorMessage = "Please select: Have you had alcohol consumption in last 48Hrs?")]
        public bool Alcohol24Hours { get; set; }

        [Required(ErrorMessage = "Please select: Have you ever engaged in any high-risk behavior?")]
        public bool RiskeyBehavior { get; set; }

        [Required(ErrorMessage = "Please select: Have you had any tattooing,ear piercing, or acupuncture in the last 06 months?")]
        public bool HasTattoOrPiercing { get; set; }

        [Required(ErrorMessage = "Please select: Have you ever tested HIV positive?")]
        public bool TestedHIVPositive { get; set; }

        [Required(ErrorMessage = "Please select: Have you ever had a history of jaundice or hepatitis?")]
        public bool HistoryOfJaundice { get; set; }

        [Required(ErrorMessage = "Please select: Have you ever been rejected previously for blood donation?")]
        public bool PrevRejectedForDonation { get; set; }

        //--Female Specific Questions
        public bool? IsPregnant { get; set; }
        public bool? IsBreastFeeding { get; set; }
        public bool? RecentDeliveryOrAbortion { get; set; }
        public bool? IsMenustruating { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DonationType == "Replacement")
            {
                if(PatientId==null || PatientId==0) /*(string.IsNullOrWhiteSpace(PatientName))*/
                {
                    yield return new ValidationResult("Patient Name is required", new[] { nameof(PatientId) });
                }
                
                if (string.IsNullOrWhiteSpace(WardNumber))
                {
                    yield return new ValidationResult("Ward Number is required", new[] { nameof(WardNumber) });
                }
            }
            if (Gender == "Female")
            {
                if (IsPregnant == null)
                {
                    yield return new ValidationResult("Please specify pregnancy status", new[] { nameof(IsPregnant) });

                }
                if (IsBreastFeeding == null)
                {
                    yield return new ValidationResult("Please specify breastfeeding status", new[] { nameof(IsBreastFeeding) });

                }
                if (RecentDeliveryOrAbortion == null)
                {
                    yield return new ValidationResult("Please specify recent delivery/abortion status", new[] { nameof(RecentDeliveryOrAbortion) });

                }
                if (IsMenustruating == null)
                {
                    yield return new ValidationResult("Please specify menstruation status", new[] { nameof(IsMenustruating) });

                }

            }

        }

        //--Physical Checkup
        [Required(ErrorMessage = "Weight is required")]
        [Range(45,120,ErrorMessage ="Weight must be between 45Kg and 120Kg")]
        [Display(Name = "Weight (Kg)")]
        public int weight { get; set; }

        [Required(ErrorMessage = "Temperature is required")]
        [Range(95, 104, ErrorMessage = "Temperature must be between 95°F and 104°F")]
        [Display(Name = "Temperature (°F)")]
        public float Temperature { get; set; }

        [Required(ErrorMessage = "Blood Pressure is required")]
        [RegularExpression(@"^\d{2,3}/\d{2,3}$",ErrorMessage ="Enter valid BP(e.g., 120/80)")]
        [Display(Name = "Blood Pressure(mmHg)")]
        public string BloodPressure { get; set; }

        [Required(ErrorMessage = "Hemoglobin level is required")]
        [Range(12, 18, ErrorMessage = "Hemoglobin must be between 12 and 18 g/dL")]
        [Display(Name = "Hemoglobin (g/dL)")]
        public float Hemoglobin { get; set; }

        [Required(ErrorMessage = "Please select: Is donor fit to donate?")]
        [Display(Name ="Fit to Donate")]
        public bool FitToDonate { get; set; }

    }
}
