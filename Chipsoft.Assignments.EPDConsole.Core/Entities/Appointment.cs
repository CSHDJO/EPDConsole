namespace Chipsoft.Assignments.EPDConsole.Core.Entities;

public class Appointment
{
    public int Id { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    
    public int PhysicianId { get; set; }
    public Physician? Physician { get; set; }
}
