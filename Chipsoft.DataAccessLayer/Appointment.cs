using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chipsoft.Data
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        // Hier bepaal ik het type van de database kolom.
        [Column(TypeName ="datetime")]
        public DateTime? StartTime { get; set; }

        public int PatientID { get; set; }
        public int PhysicianID { get; set; }

        public Patient? Patient { get; set; }
        public Physician? Physician { get; set; }
    }
}
