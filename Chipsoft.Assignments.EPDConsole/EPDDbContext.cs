using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chipsoft.Assignments.EPDConsole
{
    public class EPDDbContext : DbContext
    {
        // The following configures EF to create a Sqlite database file in the
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=epd.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
           .HasOne(a => a.Patient)
           .WithMany(p => p.Appointments)
           .HasForeignKey(a => a.PatientId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Physician)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PhysicianId);
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Physician> Physicians { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
    public class Patient
    {
        public int Id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        public string emailPatient { get; set; }
        public string passwordPatient { get; set; }

        public ICollection<Appointment> Appointments { get; set; }


    }
    public class Physician
    {
        public int Id { get; set; }

        public string emailPhysician { get; set; }

        public string passwordPhysician { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }

    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int PhysicianId { get; set; }
        public Physician Physician { get; set; }
    }

}