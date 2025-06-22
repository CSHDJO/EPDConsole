using Chipsoft.Assignments.EPDConsole.Application;
using Chipsoft.Assignments.EPDConsole.Infrastructure;
using Chipsoft.Assignments.EPDConsole.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ValidationException = Chipsoft.Assignments.EPDConsole.Application.Common.Exceptions.ValidationException;

namespace Chipsoft.Assignments.EPDConsole;

public class Program
{
    private static IServiceProvider _serviceProvider;
    private static IMediator _mediator;

    static async Task Main(string[] args)
    {
        RegisterServices();
        _mediator = _serviceProvider.GetRequiredService<IMediator>();

        await ShowMenu();
        
        DisposeServices();
    }

    private static void RegisterServices()
    {
        var services = new ServiceCollection();

        services.AddApplicationServices();
        services.AddInfrastructureServices();
        
        _serviceProvider = services.BuildServiceProvider();
    }

    private static void DisposeServices()
    {
        if (_serviceProvider == null)
        {
            return;
        }
        if (_serviceProvider is IDisposable)
        {
            ((IDisposable)_serviceProvider).Dispose();
        }
    }

    public static async Task ShowMenu()
    {
        bool continueRunning = true;
        while (continueRunning)
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
            Console.WriteLine("7 - Sluiten");
            Console.WriteLine("8 - Reset db");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        await AddPatient();
                        break;
                    case 2:
                        await DeletePatient();
                        break;
                    case 3:
                        await AddPhysician();
                        break;
                    case 4:
                        await DeletePhysician();
                        break;
                    case 5:
                        await AddAppointment();
                        break;
                    case 6:
                        await ShowAppointments();
                        break;
                    case 7:
                        continueRunning = false;
                        break;
                    case 8:
                        ResetDatabase();
                        break;
                }
            }
        }
    }

    private static async Task AddPatient()
    {
        var command = new Application.Patients.Commands.AddPatientCommand();

        Console.WriteLine("--- Nieuwe patiënt toevoegen ---");
        Console.Write("Voornaam: ");
        command.FirstName = Console.ReadLine();
        Console.Write("Achternaam: ");
        command.LastName = Console.ReadLine();
        Console.Write("BSN: ");
        command.BSN = Console.ReadLine();
        Console.Write("Adres: ");
        command.Address = Console.ReadLine();
        Console.Write("Telefoonnummer: ");
        command.PhoneNumber = Console.ReadLine();
        Console.Write("E-mail: ");
        command.Email = Console.ReadLine();

        while (true)
        {
            Console.Write("Geboortedatum (dd-mm-jjjj): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dob))
            {
                command.DateOfBirth = dob;
                break;
            }
            Console.WriteLine("Ongeldige datum, probeer opnieuw.");
        }

        try
        {
            var patientId = await _mediator.Send(command);
            Console.WriteLine($"Patiënt succesvol toegevoegd met ID: {patientId}.");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine("\nValidatie mislukt:");
            foreach (var error in ex.Errors)
            {
                Console.WriteLine($"- {error.Key}: {string.Join(", ", error.Value)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nEr is een onverwachte fout opgetreden: {ex.Message}");
        }
        
        WaitForKeyPress();
    }
    
    private static async Task DeletePatient()
    {
        Console.WriteLine("--- Patiënt verwijderen ---");

        var patients = await _mediator.Send(new Application.Patients.Queries.GetPatientsListQuery());
        if (!patients.Any())
        {
            Console.WriteLine("Er zijn geen patiënten om te verwijderen.");
            WaitForKeyPress();
            return;
        }

        Console.WriteLine("Huidige patiënten:");
        foreach (var p in patients)
        {
            Console.WriteLine($"ID: {p.Id}, Naam: {p.Name}, BSN: {p.BSN}");
        }

        Console.Write("\nVoer het ID in van de patiënt die u wilt verwijderen: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                var command = new Application.Patients.Commands.DeletePatientCommand { Id = id };
                await _mediator.Send(command);
                Console.WriteLine("Patiënt succesvol verwijderd.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij verwijderen: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Ongeldige invoer.");
        }
        WaitForKeyPress();
    }

    private static async Task AddPhysician()
    {
        Console.WriteLine("--- Nieuwe arts toevoegen ---");
        Console.Write("Voornaam: ");
        var firstName = Console.ReadLine();

        Console.Write("Achternaam: ");
        var lastName = Console.ReadLine();
        
        try
        {
            var command = new Application.Physicians.Commands.AddPhysicianCommand
            {
                FirstName = firstName,
                LastName = lastName
            };
            var physicianId = await _mediator.Send(command);
            Console.WriteLine($"Arts succesvol toegevoegd met ID: {physicianId}.");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine("\nValidatie mislukt:");
            foreach (var error in ex.Errors)
            {
                Console.WriteLine($"- {error.Key}: {string.Join(", ", error.Value)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nEr is een onverwachte fout opgetreden: {ex.Message}");
        }
        
        WaitForKeyPress();
    }
    
    private static async Task DeletePhysician()
    {
        Console.WriteLine("--- Arts verwijderen ---");

        var physicians = await _mediator.Send(new Application.Physicians.Queries.GetPhysiciansListQuery());
        if (!physicians.Any())
        {
            Console.WriteLine("Er zijn geen artsen om te verwijderen.");
            WaitForKeyPress();
            return;
        }

        Console.WriteLine("Huidige artsen:");
        foreach (var p in physicians)
        {
            Console.WriteLine($"ID: {p.Id}, Naam: {p.Name}");
        }

        Console.Write("\nVoer het ID in van de arts die u wilt verwijderen: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                var command = new Application.Physicians.Commands.DeletePhysicianCommand { Id = id };
                await _mediator.Send(command);
                Console.WriteLine("Arts succesvol verwijderd.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij verwijderen: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Ongeldige invoer.");
        }
        WaitForKeyPress();
    }

    private static async Task AddAppointment()
    {
        Console.WriteLine("--- Nieuwe afspraak toevoegen ---");

        var patients = await _mediator.Send(new Application.Patients.Queries.GetPatientsListQuery());
        if (!patients.Any())
        {
            Console.WriteLine("Er zijn geen patiënten beschikbaar. Voeg eerst een patiënt toe.");
            WaitForKeyPress();
            return;
        }

        var physicians = await _mediator.Send(new Application.Physicians.Queries.GetPhysiciansListQuery());
        if (!physicians.Any())
        {
            Console.WriteLine("Er zijn geen artsen beschikbaar. Voeg eerst een arts toe.");
            WaitForKeyPress();
            return;
        }

        Console.WriteLine("\nBeschikbare Patiënten:");
        foreach (var p in patients)
        {
            Console.WriteLine($"ID: {p.Id}, Naam: {p.Name}");
        }
        Console.Write("Kies een patiënt ID: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId) || !patients.Any(p => p.Id == patientId))
        {
            Console.WriteLine("Ongeldig patiënt ID.");
            WaitForKeyPress();
            return;
        }

        Console.WriteLine("\nBeschikbare Artsen:");
        foreach (var p in physicians)
        {
            Console.WriteLine($"ID: {p.Id}, Naam: {p.Name}");
        }
        Console.Write("Kies een arts ID: ");
        if (!int.TryParse(Console.ReadLine(), out int physicianId) || !physicians.Any(p => p.Id == physicianId))
        {
            Console.WriteLine("Ongeldig arts ID.");
            WaitForKeyPress();
            return;
        }
        
        DateTime appointmentDateTime;
        while (true)
        {
            Console.Write("Voer de datum en tijd in (dd-mm-jjjj uu:mm): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out appointmentDateTime))
            {
                break;
            }
            Console.WriteLine("Ongeldige datum/tijd, probeer opnieuw.");
        }

        try
        {
            var command = new Application.Appointments.Commands.AddAppointmentCommand
            {
                PatientId = patientId,
                PhysicianId = physicianId,
                AppointmentDateTime = appointmentDateTime
            };

            var appointmentId = await _mediator.Send(command);
            Console.WriteLine($"Afspraak succesvol toegevoegd met ID: {appointmentId}.");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine("\nValidatie mislukt:");
            foreach (var error in ex.Errors)
            {
                Console.WriteLine($"- {error.Key}: {string.Join(", ", error.Value)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nEr is een onverwachte fout opgetreden: {ex.Message}");
        }
        WaitForKeyPress();
    }
    
    private static async Task ShowAppointments()
    {
        Console.Clear();
        Console.WriteLine("--- Afspraken Inzien ---");
        Console.WriteLine("1 - Toon alle afspraken");
        Console.WriteLine("2 - Filter op patiënt");
        Console.WriteLine("3 - Filter op arts");
        Console.Write("Kies een optie: ");

        if (int.TryParse(Console.ReadLine(), out int option))
        {
            switch (option)
            {
                case 1:
                    var allAppointments = await _mediator.Send(new Application.Appointments.Queries.GetAppointmentsListQuery());
                    await DisplayAppointments(allAppointments);
                    break;
                case 2:
                    await ShowAppointmentsByPatient();
                    break;
                case 3:
                    await ShowAppointmentsByPhysician();
                    break;
                default:
                    Console.WriteLine("Ongeldige optie.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Ongeldige invoer.");
        }

        WaitForKeyPress();
    }

    private static async Task ShowAppointmentsByPatient()
    {
        var patients = await _mediator.Send(new Application.Patients.Queries.GetPatientsListQuery());
        if (!patients.Any())
        {
            Console.WriteLine("\nEr zijn geen patiënten om op te filteren.");
            return;
        }

        Console.WriteLine("\nKies een patiënt:");
        foreach (var p in patients)
        {
            Console.WriteLine($"ID: {p.Id}, Naam: {p.Name}");
        }

        Console.Write("Voer patiënt ID in: ");
        if (int.TryParse(Console.ReadLine(), out int patientId) && patients.Any(p => p.Id == patientId))
        {
            var query = new Application.Appointments.Queries.GetAppointmentsByPatientQuery { PatientId = patientId };
            var appointments = await _mediator.Send(query);
            await DisplayAppointments(appointments);
        }
        else
        {
            Console.WriteLine("Ongeldig patiënt ID.");
        }
    }

    private static async Task ShowAppointmentsByPhysician()
    {
        var physicians = await _mediator.Send(new Application.Physicians.Queries.GetPhysiciansListQuery());
        if (!physicians.Any())
        {
            Console.WriteLine("\nEr zijn geen artsen om op te filteren.");
            return;
        }

        Console.WriteLine("\nKies een arts:");
        foreach (var p in physicians)
        {
            Console.WriteLine($"ID: {p.Id}, Naam: {p.Name}");
        }

        Console.Write("Voer arts ID in: ");
        if (int.TryParse(Console.ReadLine(), out int physicianId) && physicians.Any(p => p.Id == physicianId))
        {
            var query = new Application.Appointments.Queries.GetAppointmentsByPhysicianQuery { PhysicianId = physicianId };
            var appointments = await _mediator.Send(query);
            await DisplayAppointments(appointments);
        }
        else
        {
            Console.WriteLine("Ongeldig arts ID.");
        }
    }

    private static Task DisplayAppointments(List<Application.Appointments.Dtos.AppointmentDto> appointments)
    {
        Console.WriteLine("\n--- Overzicht afspraken ---");
        if (!appointments.Any())
        {
            Console.WriteLine("Er zijn geen afspraken gevonden voor deze selectie.");
        }
        else
        {
            foreach (var app in appointments)
            {
                Console.WriteLine($"Datum/Tijd: {app.AppointmentDateTime:dd-MM-yyyy HH:mm}");
                Console.WriteLine($"  Patiënt: {app.PatientName}");
                Console.WriteLine($"  Arts: {app.PhysicianName}");
                Console.WriteLine(new string('-', 20));
            }
        }
        return Task.CompletedTask;
    }

    private static void ResetDatabase()
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EPDDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        Console.WriteLine("Database is gereset.");
        WaitForKeyPress();
    }
    
    private static void WaitForKeyPress()
    {
        Console.WriteLine("\nDruk op een toets om terug te keren naar het menu...");
        Console.ReadKey();
    }
}