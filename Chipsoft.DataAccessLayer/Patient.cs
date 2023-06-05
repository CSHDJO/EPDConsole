using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chipsoft.Data
{
    // The annotation attributes could be removed from the class to keep it simpler, and replaced with an equivalent Fluent API statement in the OnModelCreating method of the database context class.
    public class Patient
    {
        public Patient()
        {
            Appointments = new List<Appointment>();
        }

        [Key]
        public int PatientID { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;

        public int AddressID { get; set; }
        public Address? Address { get; set; }
                
        public string Contactgegeven { get; set; } = string.Empty;

        // To use Eager overloading we need to make this virtual and do some other things.
        // Using Eager overloading we can use the DbSet Include method.
        public List<Appointment> Appointments { get; set; }

        // Andere naam gekozen voor kolom in tabel
        [Column("Behandeling")]
        public string? Treatment { get; set; }
    }
}
