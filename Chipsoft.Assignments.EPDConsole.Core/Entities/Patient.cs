namespace Chipsoft.Assignments.EPDConsole.Core.Entities;

public class Patient
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BSN { get; set; } // Burger Service Nummer
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = [];
}
