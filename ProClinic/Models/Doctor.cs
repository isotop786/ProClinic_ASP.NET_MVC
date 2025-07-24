using System.ComponentModel.DataAnnotations;

namespace ProClinic.Models
{
    
    public class Doctor
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        public int Age { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required")]
        public string phone { get; set; } = "";

        [Required(ErrorMessage = "Specialization is required")]
        public string Specialization { get; set; } = "";

        public string Details { get; set; } = "";

        [Required]
        public string Gender { get; set; } = "";

        [MaxLength(1000)]
        public string ImageFileName { get; set; } = "";

        [Required(ErrorMessage = "Address is requried")]
        public string Address { get; set; } = "";

        public virtual ICollection<Patient> Patients { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
