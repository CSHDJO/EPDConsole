using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Infrastructure.Persistence;

public class EPDDbContext(DbContextOptions<EPDDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Physician> Physicians { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
