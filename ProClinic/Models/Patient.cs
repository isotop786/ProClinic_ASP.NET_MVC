using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProClinic.Models
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Patient
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

        [Required]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Address is requried")]
        public string Address { get; set; } = "";

        public int DoctorId { get; set; }  // Foreign Key

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
