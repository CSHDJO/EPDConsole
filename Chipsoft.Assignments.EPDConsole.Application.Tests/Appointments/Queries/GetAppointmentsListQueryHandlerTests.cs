using Chipsoft.Assignments.EPDConsole.Application.Appointments.Queries;
using Chipsoft.Assignments.EPDConsole.Core.Entities;
using Chipsoft.Assignments.EPDConsole.Core.Interfaces;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;


namespace Chipsoft.Assignments.EPDConsole.Application.Tests.Appointments.Queries;

public class GetAppointmentsListQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly GetAppointmentsListQueryHandler _handler;

    public GetAppointmentsListQueryHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new GetAppointmentsListQueryHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_All_Appointments_As_Dtos()
    {
        // Arrange
        var patients = new List<Patient> { new Patient { Id = 1, FirstName = "John", LastName = "Doe" } };
        var physicians = new List<Physician> { new Physician { Id = 1, FirstName = "Jane", LastName = "Smith" } };
        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, PatientId = 1, PhysicianId = 1, AppointmentDateTime = DateTime.Now.AddDays(1), Patient = patients[0], Physician = physicians[0] }
        };

        _contextMock.Setup(c => c.Appointments).ReturnsDbSet(appointments);

        var query = new GetAppointmentsListQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].PatientName.Should().Be("John Doe");
        result[0].PhysicianName.Should().Be("Jane Smith");
    }
}
