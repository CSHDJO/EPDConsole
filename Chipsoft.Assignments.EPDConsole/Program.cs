using SQLitePCL;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole
{
    public class Program
    {
        //Don't create EF migrations, use the reset db option
        //This deletes and recreates the db, this makes sure all tables exist

        private static void AddPatient()
        {
            Patient patient;
            while (true)
            {
                patient = new Patient();

                Console.WriteLine("Geef u voornaam: ");
                patient.first_name = Console.ReadLine();

                Console.WriteLine("Geef u achternaam: ");
                patient.last_name = Console.ReadLine();

                Console.WriteLine("Email: ");
                patient.emailPatient = Console.ReadLine();

                Console.WriteLine("U paswoord: ");
                patient.passwordPatient = Console.ReadLine();

                Console.WriteLine("\nU gegevens bevestigen:");
                Console.WriteLine($"Voornaam: {patient.first_name}");
                Console.WriteLine($"Achternaam: {patient.last_name}");
                Console.WriteLine($"Email: {patient.emailPatient}");
                Console.WriteLine($"Paswoord: {new string('*', patient.passwordPatient.Length)}");
                Console.WriteLine("Zijn de gegevens correct? (1. ja/2. nee/3. annuleren)");

                string confirmation = Console.ReadLine();
                int c;
                c = Int32.Parse(confirmation);
                if (c == 1)
                {
                    SavePatient(patient);
                    Console.WriteLine("Geregistreerd.");
                    return;
                }
                else if (c == 2)
                {

                    Console.WriteLine("Opnieuw proberen.\n");

                }
                else
                {
                    return;
                }
            }


        }
        static void SavePatient(Patient patient)
        {
            using (var context = new EPDDbContext())
            {
                var existingPatient = context.Patients.SingleOrDefault(p => p.emailPatient == patient.emailPatient);
                if (existingPatient != null)
                {
                    Console.WriteLine("Het e-mailadres bestaat al. Probeer in te loggen.");

                }
                else
                {
                    context.Patients.Add(patient);
                    context.SaveChanges();
                }

            }
        }
        static void SavePhysician(Physician physician)
        {
            using (var context = new EPDDbContext())
            {
                var existingPhysician = context.Physicians.SingleOrDefault(p => p.emailPhysician == physician.emailPhysician);
                if (existingPhysician != null)
                {
                    Console.WriteLine("Het e-mailadres bestaat al. Probeer in te loggen.");
                }
                else
                {
                    context.Physicians.Add(physician);
                    context.SaveChanges();
                    Console.WriteLine("Geregistreerd");
                }

            }
        }
        private static void ShowAppointmentUser(Patient z)
        {
            using (var context = new EPDDbContext())
            {
                var appointments = context.Appointments
                    .Where(a => a.PatientId == z.Id)
                    .Include(a => a.Physician)
                    .ToList();

                if (!appointments.Any())
                {
                    Console.WriteLine("Geen afspraken gevonden.");
                    return;
                }

                Console.WriteLine("Afspraken:");
                foreach (var appointment in appointments)
                {
                    Console.WriteLine($"Datum: {appointment.Date}");
                    Console.WriteLine($"Dokter: Dr. {appointment.Physician.FirstName} {appointment.Physician.LastName}");
                    Console.WriteLine($"Email: {appointment.Physician.emailPhysician}");
                    Console.WriteLine(new string('-', 30));
                }
            }
        }
        private static void ShowAppointmentPhysician(Physician z)
        {
            using (var context = new EPDDbContext())
            {
                var appointments = context.Appointments
                    .Where(a => a.PhysicianId == z.Id)
                    .Include(a => a.Patient)
                    .ToList();

                if (!appointments.Any())
                {
                    Console.WriteLine("Geen afspraken gevonden.");
                    return;
                }


                Console.WriteLine("Afspraken:");
                foreach (var appointment in appointments)
                {
                    Console.WriteLine($"Datum: {appointment.Date}");
                    Console.WriteLine($"Patient: {appointment.Patient.first_name} {appointment.Patient.last_name}");
                    Console.WriteLine($"Email: {appointment.Patient.emailPatient}");
                    Console.WriteLine(new string('-', 30));
                }
            }
        }

        private static void AddAppointment(Patient z)
        {
            using (var context = new EPDDbContext())
            {
                var physicians = context.Physicians.ToList();
                if (!physicians.Any())
                {
                    Console.WriteLine("Geen dokters beschikbaar.");
                    return;
                }

                Console.WriteLine("Selecteer een dokter uit de lijst:");
                for (int i = 0; i < physicians.Count; i++)
                {
                    var physician = physicians[i];
                    Console.WriteLine($"{i + 1}. {physician.FirstName} {physician.LastName}");
                }

                int physicianIndex;
                while (true)
                {
                    Console.Write("Geef het nummer van de gekozen dokter: ");
                    if (int.TryParse(Console.ReadLine(), out physicianIndex) && physicianIndex > 0 && physicianIndex <= physicians.Count)
                    {
                        break;
                    }
                    Console.WriteLine("Geef een geldig nummer.");
                }

                var selectedPhysician = physicians[physicianIndex - 1];

                DateTime appointmentDate;
                while (true)
                {
                    Console.Write("Geef de datum en tijd van de afspraak (vb., 2024-08-24 14:30): ");
                    if (DateTime.TryParse(Console.ReadLine(), out appointmentDate))
                    {
                        break;
                    }
                    Console.WriteLine("Ongeldige datum");
                }

                var appointment = new Appointment
                {
                    Date = appointmentDate,
                    PatientId = z.Id,
                    PhysicianId = selectedPhysician.Id
                };

                context.Appointments.Add(appointment);
                context.SaveChanges();

                Console.WriteLine("Afspraak toegevoegd");
            }

        }

        private static void DeletePhysician(Physician z)
        {
            using (var context = new EPDDbContext())
            {
                var physicians = context.Physicians
                    .Where(p => p.Id != z.Id)
                    .ToList();

                if (!physicians.Any())
                {
                    Console.WriteLine("Geen dokter gevonden");
                    return;
                }

                Console.WriteLine("Selecteer een dokter van de lijst:");
                for (int i = 0; i < physicians.Count; i++)
                {
                    var physician = physicians[i];
                    Console.WriteLine($"{i + 1}. {physician.FirstName} {physician.LastName} (Email: {physician.emailPhysician})");
                }

                int physicianIndex;
                while (true)
                {
                    Console.Write("Geeft het nummer van de juiste dokter: ");
                    if (int.TryParse(Console.ReadLine(), out physicianIndex) && physicianIndex > 0 && physicianIndex <= physicians.Count)
                    {
                        break;
                    }
                    Console.WriteLine("Ongeldig nummer.");
                }

                var selectedPhysician = physicians[physicianIndex - 1];

                Console.WriteLine($"Ben je zeker dat je deze dokter wilt verwijderen: {selectedPhysician.FirstName} {selectedPhysician.LastName}? (ja/nee)");
                var confirmation = Console.ReadLine()?.Trim().ToLower();
                if (confirmation == "ja")
                {
                    context.Physicians.Remove(selectedPhysician);
                    context.SaveChanges();
                    Console.WriteLine("Dokter verwijderd.");
                }
                else
                {
                    Console.WriteLine("Geannuleerd");
                }
            }
        }

        private static void AddPhysician()
        {
            Physician physician;
            while (true)
            {
                physician = new Physician();

                Console.WriteLine("Voornaam: ");
                physician.FirstName = Console.ReadLine();

                Console.WriteLine("Achternaam: ");
                physician.LastName = Console.ReadLine();

                Console.WriteLine("Email: ");
                physician.emailPhysician = Console.ReadLine();


                Console.WriteLine("Paswoord: ");
                physician.passwordPhysician = Console.ReadLine();

                Console.WriteLine("\nBevestig de details:");
                Console.WriteLine($"Voornaam: {physician.FirstName}");
                Console.WriteLine($"Achternaam: {physician.LastName}");
                Console.WriteLine($"Email: {physician.emailPhysician}");
                Console.WriteLine($"Paswoord: {new string('*', physician.passwordPhysician.Length)}");
                Console.WriteLine("Zijn de gegevens correct? (1. ja/2. nee/3. Annuleren)");

                string confirmation = Console.ReadLine();
                int c;
                c = Int32.Parse(confirmation);
                if (c == 1)
                {
                    SavePhysician(physician);
                    return;
                }
                else if (c == 2)
                {
                    Console.WriteLine("Opnieuw proberen.");

                }
                else
                {
                    return;
                }
            }
        }

        private static void DeletePatient()
        {
            using (var context = new EPDDbContext())
            {
                var patients = context.Patients.ToList();
                if (!patients.Any())
                {
                    Console.WriteLine("Geen patienten gevonden.");
                    return;
                }

                Console.WriteLine("Selecteer een patient van de lijst:");
                for (int i = 0; i < patients.Count; i++)
                {
                    var patient = patients[i];
                    Console.WriteLine($"{i + 1}. {patient.first_name} {patient.last_name} (Email: {patient.emailPatient})");
                }

                int patientIndex;
                while (true)
                {
                    Console.Write("Geef het nummer van de patient om te verwijderen: ");
                    if (int.TryParse(Console.ReadLine(), out patientIndex) && patientIndex > 0 && patientIndex <= patients.Count)
                    {
                        break;
                    }
                    Console.WriteLine("Ongeldige keuze. Opnieuw proberen!");
                }

                var selectedPatient = patients[patientIndex - 1];


                Console.WriteLine($"Ben je zeker dat je deze patient wilt verwijderen {selectedPatient.first_name} {selectedPatient.last_name}? (ja/nee)");
                var confirmation = Console.ReadLine()?.Trim().ToLower();
                if (confirmation == "ja")
                {

                    context.Patients.Remove(selectedPatient);
                    context.SaveChanges();
                    Console.WriteLine("Patient verwijderd.");
                }
                else
                {
                    Console.WriteLine("Geannuleerd");
                }
            }
        }
        private static void loggedInPhysician(Physician z)
        {
            Console.WriteLine($"Welkom {z.FirstName}");
            while (true)
            {
                Console.WriteLine("1. Dokter toevoegen");
                Console.WriteLine("2. Dokter verwijderen");
                Console.WriteLine("3. Patient verwijderen");
                Console.WriteLine("4. Afspraken tonen");
                Console.WriteLine("5. terug");
                string choice3 = Console.ReadLine();
                int e;

                try
                {
                    e = Int32.Parse(choice3);
                    if (e == 1)
                    {
                        AddPhysician();

                    }
                    else if (e == 2)
                    {
                        DeletePhysician(z);

                    }
                    else if (e == 3)
                    {
                        DeletePatient();

                    }
                    else if (e == 4)
                    {
                        ShowAppointmentPhysician(z);

                    }
                    else if (e == 5)
                    { return; }
                }
                catch
                {
                    Console.WriteLine("Onjuist nummer. Probeer opnieuw: ");
                    choice3 = Console.ReadLine();
                }
            }
        }
        private static void loggedInUser(Patient z)
        {
            Console.WriteLine($"Welkom {z.first_name}");
            while (true)
            {
                Console.WriteLine("1. Afspraak maken");
                Console.WriteLine("2. Afspraak tonen");
                Console.WriteLine("3. terug");


                string choice2 = Console.ReadLine();
                int d;

                try
                {
                    d = Int32.Parse(choice2);
                    if (d == 1)
                    {
                        AddAppointment(z);


                    }

                    else if (d == 2)
                    {
                        ShowAppointmentUser(z);

                    }
                    else if (d == 3)
                    {

                        return;
                    }
                }
                catch
                {
                    Console.WriteLine("Onjuist nummer probeer opnieuw: ");
                    choice2 = Console.ReadLine();
                }
            }
        }
        private static void login()
        {
            Console.WriteLine("Email address:");
            string email = Console.ReadLine();
            Console.WriteLine("Paswoord:");
            string password = Console.ReadLine();

            using (var context = new EPDDbContext())
            {
                var patient = context.Patients.SingleOrDefault(p => p.emailPatient == email);

                var physician = context.Physicians.SingleOrDefault(p => p.emailPhysician == email);


                if (patient != null && patient.passwordPatient == password)
                {
                    Console.WriteLine("Ingelogd.");
                    loggedInUser(patient);
                    return;
                }
                else if (physician != null && physician.passwordPhysician == password)
                {
                    loggedInPhysician(physician);
                }
                else
                {
                    Console.WriteLine("Ongeldig email of paswoord.");
                    return;

                }
            }

        }

        private static void logo()
        {
            try
            {
                string logo = File.ReadAllText("logo.txt");
                Console.WriteLine(logo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error lezen van file: " + ex.Message);
            }
        }
        static void Main()
        {
            using (var context = new EPDDbContext())
            {

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Physician admin = new Physician
                {
                    FirstName = "Admin",
                    LastName = "User",
                    emailPhysician = "admin@example.com",
                    passwordPhysician = "admin"
                };

                context.Physicians.Add(admin);

                context.SaveChanges();

            }

            logo();
            while (true)
            {
                Console.WriteLine("Maak u keuze: ");
                Console.WriteLine("1: Login");
                Console.WriteLine("2: Registeren");
                Console.WriteLine("3: Verlaten");

                string choice = Console.ReadLine();
                int b;

                try
                {
                    b = Int32.Parse(choice);
                    if (b == 1)
                    {
                        login();

                    }
                    else if (b == 2)
                    {
                        AddPatient();

                    }
                    else if (b == 3)
                    {
                        Console.WriteLine("Verlaten...");
                        return;

                    }
                    else
                    {
                        throw new Exception("Ongeldige keuze");

                    }

                }
                catch
                {
                    Console.WriteLine("Onjuist nummer probeer opnieuw: ");

                }
            }
        }
    }
}
