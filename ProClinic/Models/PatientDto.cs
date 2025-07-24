using System.ComponentModel.DataAnnotations;

namespace ProClinic.Models
{
    public class PatientDto
    {
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Phone { get; set; } = "";

        [Required]
        public string Symptoms { get; set; } = "";

        [Required]
        public string Gender { get; set; } = "";

        [Required(ErrorMessage = "Address is requried")]
        public string Address { get; set; } = "";

        [Required]
        public string paymentStatus { get; set; } = "Pending";

        public int DoctorId { get; set; }  // Foreign Key
    }
}
