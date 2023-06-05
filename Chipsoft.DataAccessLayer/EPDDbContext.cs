using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Data
{
    public class EPDDbContext : DbContext
    {

        // The following configures EF to create a Sqlite database file in the
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=epd.db");

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Physician> Physicians { get; set; }

        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Physician>()
                .HasKey(p => p.PhysicianID);

            modelBuilder.Entity<Physician>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<Appointment>()
                .Property(a => a.Location)
                .HasColumnType("varchar")
                .HasMaxLength(200);


            base.OnModelCreating(modelBuilder);
        }
    }

}

