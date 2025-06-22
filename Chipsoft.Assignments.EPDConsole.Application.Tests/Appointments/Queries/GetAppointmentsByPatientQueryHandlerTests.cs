using Chipsoft.Assignments.EPDConsole.Application.Appointments.Queries;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Appointments.Queries;

public class GetAppointmentsByPatientQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly GetAppointmentsByPatientQueryHandler _handler;

    public GetAppointmentsByPatientQueryHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new GetAppointmentsByPatientQueryHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Only_Appointments_For_Given_Patient()
    {
        // Arrange
        var patient1 = new Patient { Id = 1, FirstName = "John", LastName = "Doe" };
        var patient2 = new Patient { Id = 2, FirstName = "Jane", LastName = "Doe" };
        var physician = new Physician { Id = 1, FirstName = "Dr.", LastName = "Smith" };

        var appointments = new List<Appointment>
        {
            new Appointment { PatientId = 1, PhysicianId = 1, Patient = patient1, Physician = physician, AppointmentDateTime = DateTime.Now.AddDays(1) },
            new Appointment { PatientId = 2, PhysicianId = 1, Patient = patient2, Physician = physician, AppointmentDateTime = DateTime.Now.AddDays(2) },
            new Appointment { PatientId = 1, PhysicianId = 1, Patient = patient1, Physician = physician, AppointmentDateTime = DateTime.Now.AddDays(3) }
        };

        _contextMock.Setup(c => c.Appointments).ReturnsDbSet(appointments);

        var query = new GetAppointmentsByPatientQuery { PatientId = 1 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.All(a => a.PatientName == "John Doe").Should().BeTrue();
    }
}
