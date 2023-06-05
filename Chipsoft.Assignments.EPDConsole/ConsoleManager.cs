using Chipsoft.BusinessLogicLayer;
using Chipsoft.Data;

namespace Chipsoft.Assignments.EPDConsole
{
    public class ConsoleManager
    {
        private readonly PatientService _patientService;
        private readonly PhysicianService _physicianService;

        public ConsoleManager()
        {
            _physicianService = new PhysicianService();
            _patientService = new PatientService();
        }

        public ConsoleManager(PatientService serviceManager, PhysicianService physicianService)
        {
            _physicianService = physicianService;
            _patientService = serviceManager;
        }

        public bool ShowMenu()
        {
            Console.Clear();
            foreach (var line in File.ReadAllLines("logo.txt"))
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("");
            Console.WriteLine("1 - Patient toevoegen");
            Console.WriteLine("2 - Patienten verwijderen");
            Console.WriteLine("3 - Arts toevoegen");
            Console.WriteLine("4 - Arts verwijderen");
            Console.WriteLine("5 - Afspraak toevoegen");
            Console.WriteLine("6 - Afspraken inzien");
            Console.WriteLine("7 - Afspraken voor een arts inzien.");
            Console.WriteLine("8 - Afspraken voor een patient inzien.");
            Console.WriteLine("9 - Sluiten");
            Console.WriteLine("10 - Reset db");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        AddPatient();
                        return true;
                    case 2:
                        DeletePatient();
                        return true;
                    case 3:
                        AddPhysician();
                        return true;
                    case 4:
                        DeletePhysician();
                        return true;
                    case 5:
                        AddAppointment();
                        return true;
                    case 6:
                        ShowAppointments();
                        return true;
                    case 7:
                        ShowAppointmentForPhysician();
                        return true;
                    case 8:
                        ShowAppointmentForPatient();
                        return true;
                    case 9:
                        return false;
                    case 10:
                        EPDDbContext dbContext = new EPDDbContext();
                        dbContext.Database.EnsureDeleted();
                        dbContext.Database.EnsureCreated();
                        return true;
                    default:
                        return true;
                }
            }
            return true;
        }

        public void AddPatient()
        {
            Console.WriteLine("Geef de naam van de patient:");
            string? naam = Console.ReadLine();

            Console.WriteLine("Geef het contactgegeven:");
            string? contactGegeven = Console.ReadLine();

            Console.WriteLine("Geef het land:");
            string? land = Console.ReadLine();

            Console.WriteLine("Geef de stad:");
            string? stad = Console.ReadLine();

            Console.WriteLine("Geef de straat:");
            string? straat = Console.ReadLine();

            Console.WriteLine("Geef het huisnummer:");
            string? huisnummer = Console.ReadLine();

            _patientService.AddPatient(naam ?? string.Empty, contactGegeven ?? string.Empty, land ?? string.Empty, stad ?? string.Empty, straat ?? string.Empty, huisnummer ?? string.Empty);
        }

        public void DeletePatient()
        {
            Console.WriteLine("Geef de naam van de te verwijderen patient:");
            string? naam = Console.ReadLine();
            if (naam != null)
            {
                _patientService.DeletePatient(naam);
            }
        }

        public void AddPhysician()
        {
            Console.WriteLine("Geef de naam van de arts:");
            string? naam = Console.ReadLine();

            Console.WriteLine("Geef het land:");
            string? land = Console.ReadLine();

            Console.WriteLine("Geef de stad:");
            string? stad = Console.ReadLine();

            Console.WriteLine("Geef de straat:");
            string? straat = Console.ReadLine();

            Console.WriteLine("Geef het huisnummer:");
            string? huisnummer = Console.ReadLine();

            _physicianService.AddPhysician(naam ?? string.Empty, land ?? string.Empty, stad ?? string.Empty, straat ?? string.Empty, huisnummer ?? string.Empty);
        }

        public void DeletePhysician()
        {
            Console.WriteLine("Geef de naam van de te verwijderen arts:");
            string? naam = Console.ReadLine();
            if (naam != null)
            {
                _physicianService.DeletePhysician(naam);
            }
        }

        public void AddAppointment()
        {
            Console.WriteLine("Geef de naam van de arts:");
            string? naamArts = Console.ReadLine();

            Console.WriteLine("Geef de naam van de patient:");
            string? naamPatient = Console.ReadLine();

            Console.WriteLine("Geef een beschrijving van de afspraak:");
            string? beschrijving = Console.ReadLine();

            _physicianService.AddAppointment(naamArts, naamPatient, beschrijving);
        }

        public void ShowAppointments()
        {
            List<AppointmentItem> list = _physicianService.ShowAppointment();

            foreach (AppointmentItem item in list)
            {
                Console.WriteLine(item.PhysicianName + " " + item.Description + " " + item.PatientName);
            }
            Console.WriteLine("Typ om terug naar het menu te gaan:");
            string? input = Console.ReadLine();
        }

        public void ShowAppointmentForPhysician()
        {
            if (_physicianService.PhysiciansExist())
            {
                string? naamArts = string.Empty;
                bool exists = false;
                while (!exists)
                {
                    Console.WriteLine("Geef de naam van de arts:");
                    naamArts = Console.ReadLine();

                    exists = !string.IsNullOrEmpty(naamArts) && _physicianService.PhysicianExists(naamArts);
                    if (!exists) Console.WriteLine("Sorry, arts niet gevonden.");
                }

                List<AppointmentItem> list = _physicianService.ShowAppointmentForPhysician(naamArts ?? string.Empty);

                foreach (AppointmentItem item in list)
                {
                    Console.WriteLine(item.PhysicianName + " " + item.Description + " " + item.PatientName);
                }
            }
            else
            {
                Console.WriteLine("Sorry, er zijn nog geen artsen toegevoegd.");
            }

            Console.WriteLine("Typ om terug naar het menu te gaan:");
            string? input = Console.ReadLine();
        }

        public void ShowAppointmentForPatient()
        {
            Console.WriteLine("Geef de naam van de patient:");
            string? naamPatient = Console.ReadLine();

            List<AppointmentItem> list = _patientService.ShowAppointmentForPatient(naamPatient);

            foreach (AppointmentItem item in list)
            {
                // mooie plek om string interpolatie en verbatim te tonen.
                var heading = @"Arts      Beschrijving       Patient
------------------------------------";
                Console.WriteLine(heading);
                Console.WriteLine($"{item.PhysicianName} {item.Description} {item.PatientName}");
            }
            Console.WriteLine("Typ om terug naar het menu te gaan:");
            string? input = Console.ReadLine();
        }



    }
}
