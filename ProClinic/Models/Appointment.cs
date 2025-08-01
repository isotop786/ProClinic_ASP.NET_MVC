using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProClinic.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        public int? PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }

        public string? PatientName { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime  AppointmentDateTime { get; set; }

        [Required]
        [MaxLength(250)]
        public string Reason { get; set; } = "";

        public string Status { get; set; } = "Scheduled";
    }
}
