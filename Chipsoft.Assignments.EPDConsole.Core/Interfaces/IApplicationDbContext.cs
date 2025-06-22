using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace Chipsoft.Assignments.EPDConsole.Core.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Patient> Patients { get; set; }
        DbSet<Physician> Physicians { get; set; }
        DbSet<Appointment> Appointments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
} 