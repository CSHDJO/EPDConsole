namespace Chipsoft.Assignments.EPDConsole.Application.Appointments.Dtos
{
    public class AppointmentDto
    {
        public DateTime AppointmentDateTime { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PhysicianName { get; set; } = string.Empty;
    }
} 