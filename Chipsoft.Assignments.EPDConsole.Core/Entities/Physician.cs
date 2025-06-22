using System.Collections.Generic;

namespace Chipsoft.Assignments.EPDConsole.Core.Entities
{
    public class Physician
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
} 