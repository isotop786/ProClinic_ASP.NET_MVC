using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProClinic.Models
{
    public class AppointmentDto
    {

        [Required(ErrorMessage ="Doctor can not be empty") ]
        public int DoctorId { get; set; }

        public int? PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }

        public string? PatientName { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage ="Appointment date can not be empty")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDateTime { get; set; }

        [Required(ErrorMessage ="Reason must be filled")]
        [MaxLength(250)]
        public string Reason { get; set; } = "";

        public string Status { get; set; } = "Scheduled";
    }
}
